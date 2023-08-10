using System.Collections.Generic;
using UnityEngine;

public class SwitchCaseBlock : FunctionBlock {
    public List<FunctionBlock> nextBlocks;
    public OwnValueBlock valueBlock;
    public List<int> values;
    public int currentIndex;

    public List<ConnectLineController> lines;

    public override bool ExecuteFunction() {
        if (!base.ExecuteFunction())
            return false;

        for (int i = 0; i < values.Count; i++) {
            if (values[i] == valueBlock.GetOutputValue()) {
                for (int j = 0; j < GameManager.instance.ownValueBlocks.Count; j++)
                    if (nextBlocks[i] == GameManager.instance.ownValueBlocks[j])
                        OutputValue = j;
                return nextBlocks[i].ExecuteFunction();
            }
        }

        return false;
    }

    protected override void Awake() {
        base.Awake();
        currentIndex = 0;
    }

    protected override void StartConnect() {
        if (GameManager.instance.gameplayState != GameConfig.GameplayState.Playing)
            return;

        GameManager.instance.ShowPrimaryVirtualLine(gameObject, values[currentIndex].ToString());

        state = State.Connect;
    }

    protected override bool EndConnect() {
        if (!base.EndConnect())
            return false;

        CreateConnection(m_listConnection[^1].gameObject);
        return true;
    }

    private void CreateConnection(GameObject obj) {
        if (obj == null)
            return;

        while (nextBlocks.Count - 1 < currentIndex) {
            nextBlocks.Add(null);
            lines.Add(null);
        }

        if (nextBlocks[currentIndex] != null) {
            GameManager.instance.RemoveConnectLine(lines[currentIndex]);
        }
        nextBlocks[currentIndex] = obj.GetComponent<FunctionBlock>();
        lines[currentIndex] = GameManager.instance.CreateConnectPrimary(this, nextBlocks[currentIndex], values[currentIndex].ToString());

        currentIndex++;
        if (currentIndex >= values.Count)
            currentIndex = 0;
    }
}