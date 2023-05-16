using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [Header("Lines")]
    public AnimatedLineController resultDrawLine;

    public VirtualLine primaryVirtualLine;
    public VirtualLine secondaryVirtualLine;
    public ConnectLineController primaryConnectLine;
    public ConnectLineController secondaryConnectLine;

    [Header("Testing")]
    public StartBlock startBlock;

    public List<TestCase> testCases;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
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
            Debug.Log("You Won!");
        else {
            Debug.Log("Opps, try again");

            yield return new WaitForSeconds(GameConfig.VISUALIZE_GAP_RESULT_DURATION);

            foreach (var testCase in testCases)
                testCase.ResetState();
        }
    }
}