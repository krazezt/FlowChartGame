using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static TestCase;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameConfig.AppType appType;
    public GameConfig.GameplayState gameplayState;

    public Image testCasesPanel;
    public VariablesPanel variablesPanel;
    public GameObject blocksHolder;

    public AnimatedLineController resultDrawLine;
    public VirtualLine primaryVirtualLine;
    public VirtualLine secondaryVirtualLine;
    public ConnectLineController primaryConnectLine;
    public ConnectLineController secondaryConnectLine;

    public List<VariableBlock> variableBlockPrefabs;
    public List<FunctionBlock> functionBlockPrefabs;
    public List<TestCase> testCasePrefabs;

    public List<VariableBlock> variableBlocks;
    public List<FunctionBlock> functionBlocks;
    public List<OwnValueBlock> ownValueBlocks;
    public List<TestCase> testCases;
    public List<CasePair> customCasePairs;
    public List<ConnectLineController> connectLines;

    public int generateGridWidth = 3;
    public int generateGridHeight = 3;
    public const float GRID_CELL_SIZE = 2.5f;
    public bool onWaiting;
    public LevelDataDTO levelData;

    [Header("Testing")]
    public StartBlock startBlock;

    private Queue<List<int>> variableLog;
    private static bool isOutStackRange;
    private int prevLevel = -1;
    private int currentLevel;

    private void Awake() {
        instance = this;

        variableLog = new();
        connectLines = new();

        Application.targetFrameRate = 60;
        prevLevel = -1;
        currentLevel = -1;
        onWaiting = false;
    }

    private void LoadLevel() {
        StartCoroutine(LoadLevelCoroutine());
    }

    private IEnumerator LoadLevelCoroutine() {
        prevLevel = currentLevel;
        onWaiting = true;
        UIManager.instance.ShowLoadingScreen();

        string apiRoute = "https://flow-chart-game-server.vercel.app/gameplay/data/get-random-level";
        WWWForm form = new();
        form.AddField("except", prevLevel);

        using UnityWebRequest www = UnityWebRequest.Post(apiRoute, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        } else {
            ClearLevel();
            levelData = JsonConvert.DeserializeObject<LevelDataDTO>(www.downloadHandler.text);
            InitLevel(levelData);
            currentLevel = levelData.index;
        }

        //UIManager.instance.HidePopup();
        UIManager.instance.HideLoadingScreen();
        UIManager.instance.ShowRequestPopup();
        onWaiting = false;
    }

    private void ClearLevel() {
        ClearResultPoint();
        variablesPanel.ClearVariables();
        customCasePairs.Clear();

        foreach (var testcase in testCases)
            Destroy(testcase.gameObject);
        testCases.Clear();

        foreach (var line in connectLines)
            Destroy(line.gameObject);
        connectLines.Clear();

        foreach (var block in variableBlocks)
            Destroy(block.gameObject);
        variableBlocks.Clear();

        foreach (var block in functionBlocks)
            Destroy(block.gameObject);

        functionBlocks.Clear();

        foreach (var block in ownValueBlocks)
            Destroy(block.gameObject);

        ownValueBlocks.Clear();
    }

    public void LoadNextLevel() {
        LoadLevel();
    }

    private void Start() {
        LoadLevel();
    }

    public void ShowPrimaryVirtualLine(GameObject startPoint, bool displayLabel = false) {
        primaryVirtualLine.SetStartPoint(startPoint);
        primaryVirtualLine.Show(displayLabel);
    }

    public void ShowPrimaryVirtualLine(GameObject startPoint, string label) {
        primaryVirtualLine.SetStartPoint(startPoint);
        primaryVirtualLine.Show(label);
    }

    public void ShowSecondaryVirtualLine(GameObject startPoint, bool displayLabel = false) {
        secondaryVirtualLine.SetStartPoint(startPoint);
        secondaryVirtualLine.Show(displayLabel);
    }

    public void UpdateCustomCasePairValues() {
        foreach (var pair in customCasePairs) {
            try {
                pair.value = int.Parse(pair.variableBlock.valueText.text);
            } catch {
                pair.value = 0;
            }
        }
    }

    public void HideVirtualLine() {
        primaryVirtualLine.Hide();
        secondaryVirtualLine.Hide();
    }

    public bool AppendResultLinePoint(GameObject obj) {
        resultDrawLine.AddPoint(obj);

        EnqueueVariableLog();

        if (variableLog.Count > GameConfig.MAX_STEP) {
            isOutStackRange = true;
            return false;
        } else
            return true;
    }

    public void EnqueueVariableLog() {
        List<int> newLog = new();
        foreach (VariableBlock variable in variableBlocks) {
            newLog.Add(variable.GetOutputValue());
        }
        variableLog.Enqueue(newLog);
    }

    public void DequeueVariableLog() {
        List<int> varLog = variableLog.Dequeue();

        for (int i = 0; i < varLog.Count; i++) {
            variableBlocks[i].DisplayValue(varLog[i]);
        }
    }

    public void InitLevel(LevelDataDTO levelData) {
        InitVariables(levelData.variables);
        InitFunctionBlocks(levelData.functionBlocks);
        InitTestCases(levelData.testCases);

        UIManager.instance.SetRequestText(levelData.request);
        variablesPanel.SetVariables(variableBlocks, (customCasePairs.Select((ele) => ele.variableBlock)).ToList());
        variablesPanel.Hide();

        UIManager.instance.OnStopSimulate();
        gameplayState = GameConfig.GameplayState.Playing;
    }

    public Vector3 GetInitPosition(int index) {
        Vector2 coord = new() {
            x = index % generateGridWidth,
            y = index / generateGridHeight,
        };

        return new(-GRID_CELL_SIZE + coord.x * GRID_CELL_SIZE, GRID_CELL_SIZE - coord.y * GRID_CELL_SIZE, 0);
    }

    public void InitVariables(List<VariableDTO> variables) {
        int i = 0;
        foreach (VariableDTO variable in variables) {
            VariableBlock variableBlock = Instantiate(variableBlockPrefabs[variable.variableTypeID].gameObject).GetComponent<VariableBlock>();
            variableBlock.SetLabel(variable.variableName);
            variableBlock.variableName = variable.variableName;
            variableBlock.DisplayValue();

            variableBlocks.Add(variableBlock);
            ownValueBlocks.Add(variableBlock);

            // Config
            RectTransform rectTransform =  variableBlock.GetComponent<RectTransform>();
            rectTransform.SetParent(variablesPanel.GetComponent<RectTransform>());
            rectTransform.anchoredPosition = new Vector3(0, -90, 0) + (i++) * 180 * new Vector3(0, -1, 0);
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = new(150, 165);
        }
    }

    public void InitFunctionBlocks(List<FunctionBlockDTO> functionBlockDTOs) {
        // Add blocks to the manage lists.
        int index = 0;
        foreach (FunctionBlockDTO functionBlockDTO in functionBlockDTOs) {
            FunctionBlock block = Instantiate(functionBlockPrefabs[functionBlockDTO.blockTypeID].gameObject).GetComponent<FunctionBlock>();
            block.SetLabel(functionBlockDTO.text);
            block.transform.parent = blocksHolder.transform;
            block.transform.position = GetInitPosition(index++);
            functionBlocks.Add(block);
            ownValueBlocks.Add(block);
        }

        // Init values for the blocks
        for (int i = 0; i < functionBlockDTOs.Count; i++) {
            switch ((GameConfig.FunctionBlockType)functionBlockDTOs[i].blockTypeID) {
                case GameConfig.FunctionBlockType.Start:
                    startBlock = (functionBlocks[i] as StartBlock);
                    break;

                case GameConfig.FunctionBlockType.Action:
                    break;

                case GameConfig.FunctionBlockType.Assign:
                    (functionBlocks[i] as AssignBlock).VariableBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]] as VariableBlock;
                    (functionBlocks[i] as AssignBlock).ValueBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    break;

                case GameConfig.FunctionBlockType.ConditionEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    break;

                case GameConfig.FunctionBlockType.ConditionLess:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.LESS;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    break;

                case GameConfig.FunctionBlockType.ConditionLessEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.LESS_OR_EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    break;

                case GameConfig.FunctionBlockType.ConditionGreater:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.GREATER;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    break;

                case GameConfig.FunctionBlockType.ConditionGreaterEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.GREATER_OR_EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    break;

                case GameConfig.FunctionBlockType.Add:
                    (functionBlocks[i] as OperationBlock).operatorType = OperationBlock.Operator.ADD;
                    (functionBlocks[i] as OperationBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as OperationBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as OperationBlock).OutputBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[2]] as VariableBlock;
                    break;

                case GameConfig.FunctionBlockType.Subtract:
                    (functionBlocks[i] as OperationBlock).operatorType = OperationBlock.Operator.SUBTRACT;
                    (functionBlocks[i] as OperationBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as OperationBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as OperationBlock).OutputBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[2]] as VariableBlock;
                    break;

                case GameConfig.FunctionBlockType.Multiply:
                    (functionBlocks[i] as OperationBlock).operatorType = OperationBlock.Operator.MULTIPLY;
                    (functionBlocks[i] as OperationBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as OperationBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as OperationBlock).OutputBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[2]] as VariableBlock;
                    break;

                case GameConfig.FunctionBlockType.Divide:
                    (functionBlocks[i] as OperationBlock).operatorType = OperationBlock.Operator.DIVIDE;
                    (functionBlocks[i] as OperationBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as OperationBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as OperationBlock).OutputBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[2]] as VariableBlock;
                    break;

                case GameConfig.FunctionBlockType.EndBlock:
                    (functionBlocks[i] as EndBlock).checkValue = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as EndBlock).validateURL = levelData.validateURL;
                    for (int j = 1; j < functionBlockDTOs[i].connectBlocks.Count; j++) {
                        (functionBlocks[i] as EndBlock).inputValues.Add(ownValueBlocks[functionBlockDTOs[i].connectBlocks[j]]);
                    }
                    break;

                case GameConfig.FunctionBlockType.Increase:
                    (functionBlocks[i] as IncreaseBlock).VariableBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]] as VariableBlock;
                    break;

                case GameConfig.FunctionBlockType.Decrease:
                    (functionBlocks[i] as DecreaseBlock).VariableBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]] as VariableBlock;
                    break;

                case GameConfig.FunctionBlockType.SwitchCase:
                    (functionBlocks[i] as SwitchCaseBlock).valueBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as SwitchCaseBlock).values = functionBlockDTOs[i].values;
                    break;
            }
        }
    }

    public void InitTestCases(List<TestCaseDTO> testCaseDTOs) {
        foreach (CasePairDTO pairDTO in testCaseDTOs[0].casePairs) {
            customCasePairs.Add(new CasePair() {
                value = 0,
                variableBlock = variableBlocks[pairDTO.variableBlockIndex],
            });
        }

        int i = 0;
        foreach (TestCaseDTO testCaseDTO in testCaseDTOs) {
            TestCase testCase = Instantiate(testCasePrefabs[testCaseDTO.testCaseTypeID].gameObject).GetComponent<TestCase>();

            // Config
            RectTransform rectTransform =  testCase.GetComponent<RectTransform>();
            rectTransform.SetParent(testCasesPanel.GetComponent<RectTransform>());
            rectTransform.anchoredPosition = new Vector3(120, 0, 0) + (i++) * 190 * new Vector3(1, 0, 0);
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = 140 * Vector2.one;

            testCase.SetLabel(testCaseDTO.text);
            testCase.casePairs = new();
            foreach (CasePairDTO casePairDTO in testCaseDTO.casePairs) {
                CasePair casePair = new() {
                    value = casePairDTO.value,
                    variableBlock = variableBlocks[casePairDTO.variableBlockIndex]
                };

                testCase.casePairs.Add(casePair);
            }

            testCases.Add(testCase);
        }
    }

    public void ClearResultPoint() {
        resultDrawLine.ClearPoints();
        variableLog.Clear();
    }

    public ConnectLineController CreateConnectPrimary(FunctionBlock from, FunctionBlock to, bool displayLabel = false) {
        ConnectLineController newLine = Instantiate(primaryConnectLine.gameObject).GetComponent<ConnectLineController>();

        newLine.startPoint = from.gameObject;
        newLine.endPoint = to.gameObject;
        newLine.labelText.SetActive(displayLabel);

        connectLines.Add(newLine);
        HideVirtualLine();
        return newLine;
    }

    public ConnectLineController CreateConnectPrimary(FunctionBlock from, FunctionBlock to, string label) {
        ConnectLineController newLine = Instantiate(primaryConnectLine.gameObject).GetComponent<ConnectLineController>();

        newLine.startPoint = from.gameObject;
        newLine.endPoint = to.gameObject;
        newLine.labelText.GetComponentInChildren<TMP_Text>().text = label;
        newLine.labelText.SetActive(true);

        connectLines.Add(newLine);
        HideVirtualLine();
        return newLine;
    }

    public ConnectLineController CreateConnectSecondary(FunctionBlock from, FunctionBlock to, bool displayLabel = false) {
        ConnectLineController newLine = Instantiate(secondaryConnectLine.gameObject).GetComponent<ConnectLineController>();

        newLine.startPoint = from.gameObject;
        newLine.endPoint = to.gameObject;
        newLine.labelText.SetActive(displayLabel);

        connectLines.Add(newLine);
        HideVirtualLine();
        return newLine;
    }

    public void RemoveConnectLine(ConnectLineController line) {
        connectLines.Remove(line);
        Destroy(line.gameObject);
    }

    public void Pause() {
        Time.timeScale = 0f;
    }

    public void UnPause() {
        Time.timeScale = 1f;
    }

    public void StartTest() {
        if (gameplayState == GameConfig.GameplayState.Simulating)
            return;

        isOutStackRange = false;
        switch (variablesPanel.CurrentState) {
            case VariablesPanel.State.ShowingCustomables:
                StartTestCustomCase();
                break;

            default:
                StartTestAllCases();
                break;
        }

        UIManager.instance.OnStartSimulate();
        EndBlock.isAllConditionTrue = true;
        EndBlock.requestingCount = 0;
        gameplayState = GameConfig.GameplayState.Simulating;
    }

    private void StartTestAllCases() {
        resultDrawLine.Hide();
        variablesPanel.Show(VariablesPanel.State.ShowingAll);
        StartCoroutine(TestAllCases());
    }

    private void StartTestCustomCase() {
        ClearResultPoint();
        resultDrawLine.Hide();
        variablesPanel.Show(VariablesPanel.State.ShowingAll);
        foreach (CasePair casePair in customCasePairs) {
            casePair.variableBlock.AssignOutputValue(casePair.value);
        }

        StartCoroutine(TestCustomCasePair());
    }

    private IEnumerator TestCustomCasePair() {
        try {
            startBlock.ExecuteFunction();
        } catch { }

        resultDrawLine.StartAnimateLine();
        while (resultDrawLine.OnAnimating || EndBlock.requestingCount > 0)
            yield return null;

        yield return new WaitForSeconds(GameConfig.VISUALIZE_GAP_RESULT_DURATION);
        variablesPanel.ResetVariables();
        variablesPanel.Hide();

        UIManager.instance.OnStopSimulate();
        gameplayState = GameConfig.GameplayState.Playing;
    }

    public void StopSimulation() {
        StopAllCoroutines();

        foreach (var testCase in testCases)
            testCase.ResetState();

        resultDrawLine.StopAnimating();
        variablesPanel.ResetVariables();
        variablesPanel.Hide();
        UIManager.instance.OnStopSimulate();
        gameplayState = GameConfig.GameplayState.Playing;
    }

    private IEnumerator TestAllCases() {
        bool isWin = true;

        foreach (var testCase in testCases) {
            variablesPanel.ResetVariables();
            ClearResultPoint();
            testCase.SetupTestCase();
            testCase.MarkAsChecking();

            bool passed;
            try {
                passed = startBlock.ExecuteFunction();
            } catch {
                passed = false;
                isWin = false;
            }

            Debug.Log("passed: " + passed + ", isWin: " + isWin);
            resultDrawLine.StartAnimateLine();
            while (resultDrawLine.OnAnimating || EndBlock.requestingCount > 0)
                yield return null;

            if (!passed || !EndBlock.isAllConditionTrue) {
                isWin = false;
                testCase.MarkAsFailed();
            } else {
                testCase.MarkAsPassed();
            }

            yield return new WaitForSeconds(GameConfig.VISUALIZE_GAP_SEGMENT_DURATION);
        }

        if (isWin) {
            variablesPanel.Hide();
            AudioManager.instance.PlaySFX(AudioConfig.Track.Win);
            UIManager.instance.ShowPopupDelay(UIManager.Popup.Win, 0f);

            UIManager.instance.OnStopSimulate();
            gameplayState = GameConfig.GameplayState.Playing;
        } else {
            if (isOutStackRange)
                Debug.Log("Out stack range!");
            else
                Debug.Log("Not true, try again!");

            yield return new WaitForSeconds(GameConfig.VISUALIZE_GAP_RESULT_DURATION);

            variablesPanel.Hide();

            UIManager.instance.OnStopSimulate();
            gameplayState = GameConfig.GameplayState.Playing;

            foreach (var testCase in testCases)
                testCase.ResetState();
        }
    }
}