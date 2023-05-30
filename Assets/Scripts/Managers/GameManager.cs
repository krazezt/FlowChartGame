using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameConfig.AppType appType;

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
    }

    private LevelDataDTO InitTestLevelData() {
        levelData = gameObject.GetOrAddComponent<RWFile>().ReadFileData<LevelDataDTO>("Data/test");
        Debug.Log(levelData.Variables.Count);

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
        InitVariables(levelData.Variables);
        InitFunctionBlocks(levelData.FunctionBlocks);
        InitTestCases(levelData.TestCases);
    }

    public void InitVariables(List<int> variableIDs) {
        foreach (int variableID in variableIDs) {
            VariableBlock block = Instantiate(variableBlockPrefabs[variableID].gameObject).GetComponent<VariableBlock>();
            variableBlocks.Add(block);
            ownValueBlocks.Add(block);
        }
    }

    public void InitFunctionBlocks(List<FunctionBlockDTO> functionBlockDTOs) {
        foreach (FunctionBlockDTO functionBlockDTO in functionBlockDTOs) {
            Debug.Log("BlockTypeID: " + functionBlockDTO.BlockTypeID + ", Count: " + functionBlockPrefabs.Count);
            FunctionBlock block = Instantiate(functionBlockPrefabs[functionBlockDTO.BlockTypeID].gameObject).GetComponent<FunctionBlock>();
            functionBlocks.Add(block);
            ownValueBlocks.Add(block);
        }

        for (int i = 0; i < functionBlockDTOs.Count; i++) {
            switch ((GameConfig.FunctionBlockType)functionBlockDTOs[i].BlockTypeID) {
                case GameConfig.FunctionBlockType.Start:
                    (functionBlocks[i] as StartBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].NextBlocks[0]] as FunctionBlock;
                    startBlock = (functionBlocks[i] as StartBlock);
                    break;

                case GameConfig.FunctionBlockType.Action:
                    (functionBlocks[i] as ActionBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].NextBlocks[0]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.Assign:
                    (functionBlocks[i] as AssignBlock).VariableBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[0]] as VariableBlock;
                    (functionBlocks[i] as AssignBlock).ValueBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[1]];
                    (functionBlocks[i] as AssignBlock).NextBlock = ownValueBlocks[functionBlockDTOs[i].NextBlocks[0]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[2]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[3]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionLess:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.LESS;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[2]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[3]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionLessEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.LESS_OR_EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[2]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[3]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionGreater:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.GREATER;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[2]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[3]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.ConditionGreaterEqual:
                    (functionBlocks[i] as ConditionBlock).operatorType = ConditionBlock.Operator.GREATER_OR_EQUAL;
                    (functionBlocks[i] as ConditionBlock).LeftSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[0]];
                    (functionBlocks[i] as ConditionBlock).RightSideOperandBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[1]];
                    (functionBlocks[i] as ConditionBlock).TrueConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[2]] as FunctionBlock;
                    (functionBlocks[i] as ConditionBlock).FalseConditionBlock = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[3]] as FunctionBlock;
                    break;

                case GameConfig.FunctionBlockType.EndMax:
                    (functionBlocks[i] as EBMax).checkValue = ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[0]];
                    for (int j = 1; j < functionBlockDTOs[i].ConnectBlocks.Count; j++) {
                        (functionBlocks[i] as EBMax).inputValues.Add(ownValueBlocks[functionBlockDTOs[i].ConnectBlocks[j]]);
                    }
                    break;
            }
        }
    }

    public void InitTestCases(List<TestCaseDTO> testCaseDTOs) {
        foreach (TestCaseDTO testCaseDTO in testCaseDTOs) {
            testCases.Add(Instantiate(testCasePrefabs[testCaseDTO.TestCaseTypeID].gameObject).GetComponent<TestCase>());
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
                yield return new WaitForSeconds(GameConfig.VISUALIZE_SEGMENT_DURATION * resultDrawLine.linePoints.Count - 1);
                testCase.MarkAsFailed();
            } else {
                resultDrawLine.StartAnimateLine();
                yield return new WaitForSeconds(GameConfig.VISUALIZE_SEGMENT_DURATION * resultDrawLine.linePoints.Count - 1);
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