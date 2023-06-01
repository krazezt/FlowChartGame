using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Z.Expressions;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameConfig.AppType appType;

    public Image testCasesPanel;

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
    public List<TestCase.CasePair> customCasePairs;

    public LevelDataDTO levelData;

    [Header("Testing")]
    public StartBlock startBlock;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        List<int> list = new(){0, 1, 2, 3};
        string funcContent = @"
            int result = 0;
            for (int i = 0; i < list.Count; i++) {
                result += list[i];
            }

            return result;
        ";

        Debug.Log(Eval.Execute<int>(funcContent, new {
            list
        }));
    }

    private LevelDataDTO InitTestLevelData() {
        levelData = gameObject.GetOrAddComponent<RWFile>().ReadFileData<LevelDataDTO>("Data/UCLN_AB");
        Debug.Log(levelData.variables.Count);

        return levelData;
    }

    private void Start() {
        InitTestLevelData();
        InitLevel(levelData);
    }

    public void ShowPrimaryVirtualLine(Vector3 startPoint) {
        primaryVirtualLine.SetStartPoint(startPoint);
        primaryVirtualLine.Show();
    }

    public void ShowSecondaryVirtualLine(Vector3 startPoint) {
        secondaryVirtualLine.SetStartPoint(startPoint);
        secondaryVirtualLine.Show();
    }

    public void HideVirtualLine() {
        primaryVirtualLine.Hide();
        secondaryVirtualLine.Hide();
    }

    public void AppendResultLinePoint(GameObject obj) {
        resultDrawLine.AddPoint(obj);
    }

    public void InitLevel(LevelDataDTO levelData) {
        InitVariables(levelData.variables);
        InitFunctionBlocks(levelData.functionBlocks);
        InitTestCases(levelData.testCases);
    }

    public void InitVariables(List<int> variableIDs) {
        foreach (int variableID in variableIDs) {
            VariableBlock block = Instantiate(variableBlockPrefabs[variableID].gameObject).GetComponent<VariableBlock>();
            variableBlocks.Add(block);
            ownValueBlocks.Add(block);
        }
    }

    public void InitFunctionBlocks(List<FunctionBlockDTO> functionBlockDTOs) {
        // Add blocks to the manage lists.
        foreach (FunctionBlockDTO functionBlockDTO in functionBlockDTOs) {
            FunctionBlock block = Instantiate(functionBlockPrefabs[functionBlockDTO.blockTypeID].gameObject).GetComponent<FunctionBlock>();
            block.SetLabel(functionBlockDTO.text);
            functionBlocks.Add(block);
            ownValueBlocks.Add(block);
        }

        // Init values for the blocks
        for (int i = 0; i < functionBlockDTOs.Count; i++) {
            switch ((GameConfig.FunctionBlockType)functionBlockDTOs[i].blockTypeID) {
                case GameConfig.FunctionBlockType.Start:
                    (functionBlocks[i] as StartBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    startBlock = (functionBlocks[i] as StartBlock);
                    break;

                case GameConfig.FunctionBlockType.Action:
                    (functionBlocks[i] as ActionBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.Assign:
                    (functionBlocks[i] as AssignBlock).VariableBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]] as VariableBlock;
                    (functionBlocks[i] as AssignBlock).ValueBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as AssignBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[1]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionLess:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.LESS;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[1]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionLessEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.LESS_OR_EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[1]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionGreater:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.GREATER;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[1]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionGreaterEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.GREATER_OR_EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[1]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.Add:
                    (functionBlocks[i] as OperationBlock).operatorType = OperationBlock.Operator.ADD;
                    (functionBlocks[i] as OperationBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as OperationBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as OperationBlock).OutputBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[2]] as VariableBlock;
                    (functionBlocks[i] as OperationBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.Subtract:
                    (functionBlocks[i] as OperationBlock).operatorType = OperationBlock.Operator.SUBTRACT;
                    (functionBlocks[i] as OperationBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as OperationBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as OperationBlock).OutputBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[2]] as VariableBlock;
                    (functionBlocks[i] as OperationBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.Multiply:
                    (functionBlocks[i] as OperationBlock).operatorType = OperationBlock.Operator.MULTIPLY;
                    (functionBlocks[i] as OperationBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as OperationBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as OperationBlock).OutputBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[2]] as VariableBlock;
                    (functionBlocks[i] as OperationBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.Divide:
                    (functionBlocks[i] as OperationBlock).operatorType = OperationBlock.Operator.DIVIDE;
                    (functionBlocks[i] as OperationBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as OperationBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[1]];
                    (functionBlocks[i] as OperationBlock).OutputBlock = ownValueBlocks[functionBlockDTOs[i].connectBlocks[2]] as VariableBlock;
                    (functionBlocks[i] as OperationBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].nextBlocks[0]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.End:
                    (functionBlocks[i] as EndBlock).checkValue = ownValueBlocks[functionBlockDTOs[i].connectBlocks[0]];
                    (functionBlocks[i] as EndBlock).validateCode = levelData.validateCode;
                    for (int j = 1; j < functionBlockDTOs[i].connectBlocks.Count; j++) {
                        (functionBlocks[i] as EndBlock).inputValues.Add(ownValueBlocks[functionBlockDTOs[i].connectBlocks[j]]);
                    }
                    break;
            }
        }
    }

    public void InitTestCases(List<TestCaseDTO> testCaseDTOs) {
        foreach (CasePairDTO pairDTO in testCaseDTOs[0].casePairs) {
            customCasePairs.Add(new TestCase.CasePair() {
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
                TestCase.CasePair casePair = new() {
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
    }

    public ConnectLineController CreateConnectPrimary(FunctionBlock from, FunctionBlock to) {
        GameObject newLine = Instantiate(primaryConnectLine.gameObject);

        newLine.GetComponent<ConnectLineController>().startPoint = from.gameObject;
        newLine.GetComponent<ConnectLineController>().endPoint = to.gameObject;

        HideVirtualLine();
        return newLine.GetComponent<ConnectLineController>();
    }

    public ConnectLineController CreateConnectSecondary(FunctionBlock from, FunctionBlock to) {
        GameObject newLine = Instantiate(secondaryConnectLine.gameObject);

        newLine.GetComponent<ConnectLineController>().startPoint = from.gameObject;
        newLine.GetComponent<ConnectLineController>().endPoint = to.gameObject;

        HideVirtualLine();
        return newLine.GetComponent<ConnectLineController>();
    }

    public void StartTestAllCases() {
        resultDrawLine.Hide();
        StartCoroutine(TestAllCases());
    }

    public void StartTestCustomCasse() {
        resultDrawLine.Hide();
        //StartCoroutine(TestAllCases());
    }

    public IEnumerator TestAllCases() {
        bool isWin = true;

        foreach (var testCase in testCases) {
            testCase.SetupTestCase();
            testCase.MarkAsChecking();

            bool passed;
            try {
                passed = startBlock.ExecuteFunction();
            } catch {
                passed = false;
                isWin = false;
            }

            if (!passed) {
                isWin = false;
                resultDrawLine.StartAnimateLine();
                while (resultDrawLine.OnAnimating)
                    yield return null;
                testCase.MarkAsFailed();
            } else {
                resultDrawLine.StartAnimateLine();
                while (resultDrawLine.OnAnimating)
                    yield return null;
                testCase.MarkAsPassed();
            }

            yield return new WaitForSeconds(GameConfig.VISUALIZE_GAP_SEGMENT_DURATION);
        }

        if (isWin)
            UIManager.instance.ShowPopupDelay(UIManager.Popup.Win, 1f);
        else {
            Debug.Log("Opps, try again");

            yield return new WaitForSeconds(GameConfig.VISUALIZE_GAP_RESULT_DURATION);

            foreach (var testCase in testCases)
                testCase.ResetState();
        }
    }
}