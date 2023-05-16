using UnityEngine;

public class ActionBlock : FunctionBlock {
    public FunctionBlock NextBlock;
    [HideInInspector] public ConnectLineController lineToNext;

    public override bool ExecuteFunction() {
        base.ExecuteFunction();

        return NextBlock.ExecuteFunction();
    }

    protected override bool EndConnect() {
        if (!base.EndConnect())
            return false;

        NextBlock = m_listConnection[^1].gameObject.GetComponent<FunctionBlock>();

        if (lineToNext != null)
            Destroy(lineToNext.gameObject);

        lineToNext = GameManager.instance.CreateConnectPrimary(this, NextBlock);

        return true;
    }
}