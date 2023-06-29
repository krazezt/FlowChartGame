using UnityEngine;

public class ActionBlock : FunctionBlock {
    public FunctionBlock NextBlock;
    [HideInInspector] public ConnectLineController lineToNext;

    public override bool ExecuteFunction() {
        if (!base.ExecuteFunction())
            return false;

        return NextBlock.ExecuteFunction();
    }

    protected override bool EndConnect() {
        if (!base.EndConnect())
            return false;

        NextBlock = m_listConnection[^1].gameObject.GetComponent<FunctionBlock>();

        if (lineToNext != null) {
            GameManager.instance.RemoveConnectLine(lineToNext);
        }

        lineToNext = GameManager.instance.CreateConnectPrimary(this, NextBlock);

        return true;
    }
}