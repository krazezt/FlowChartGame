using UnityEngine;

public class ConditionBlock : FunctionBlock {

    public enum Operator {
        LESS,
        GREATER,
        LESS_OR_EQUAL,
        GREATER_OR_EQUAL,
        EQUAL,
    }

    public FunctionBlock TrueConditionBlock;
    public FunctionBlock FalseConditionBlock;

    public ConnectLineController lineToTrue;
    public ConnectLineController lineToFalse;

    public OwnValueBlock LeftSideOperandBlock;
    public OwnValueBlock RightSideOperandBlock;
    public Operator operatorType;

    public override bool ExecuteFunction() {
        if (!base.ExecuteFunction())
            return false;

        if (CheckCondition()) {
            Debug.Log("True, " + operatorType + ", Left: " + LeftSideOperandBlock.GetOutputValue() + ", right: " + RightSideOperandBlock.GetOutputValue());
            return TrueConditionBlock.ExecuteFunction();
        } else {
            Debug.Log("False, " + operatorType + ", Left: " + LeftSideOperandBlock.GetOutputValue() + ", right: " + RightSideOperandBlock.GetOutputValue());
            return FalseConditionBlock.ExecuteFunction();
        }
    }

    protected override void Awake() {
        base.Awake();

        TrueConditionBlock = null;
        FalseConditionBlock = null;
        lineToTrue = null;
        lineToFalse = null;
    }

    protected bool CheckCondition() {
        switch (operatorType) {
            case Operator.EQUAL:
                return LeftSideOperandBlock.GetOutputValue() == RightSideOperandBlock.GetOutputValue();

            case Operator.LESS:
                return LeftSideOperandBlock.GetOutputValue() < RightSideOperandBlock.GetOutputValue();

            case Operator.GREATER:
                return LeftSideOperandBlock.GetOutputValue() > RightSideOperandBlock.GetOutputValue();

            case Operator.LESS_OR_EQUAL:
                return LeftSideOperandBlock.GetOutputValue() <= RightSideOperandBlock.GetOutputValue();

            case Operator.GREATER_OR_EQUAL:
                return LeftSideOperandBlock.GetOutputValue() <= RightSideOperandBlock.GetOutputValue();

            default:
                return false;
        }
    }

    protected override void StartConnect() {
        if (GameManager.instance.gameplayState != GameConfig.GameplayState.Playing)
            return;

        if (TrueConditionBlock == null)
            GameManager.instance.ShowPrimaryVirtualLine(gameObject, true);
        else if (FalseConditionBlock == null)
            GameManager.instance.ShowSecondaryVirtualLine(gameObject, true);
        else
            GameManager.instance.ShowPrimaryVirtualLine(gameObject, true);

        state = State.Connect;
    }

    protected override bool EndConnect() {
        if (!base.EndConnect())
            return false;

        if (TrueConditionBlock == null) {
            SetTrueConnection(m_listConnection[^1].gameObject);
        } else if (FalseConditionBlock == null) {
            SetFalseConnection(m_listConnection[^1].gameObject);
        } else {
            SetFalseConnection(null);
            SetTrueConnection(m_listConnection[^1].gameObject);
        }

        return true;
    }

    private void SetTrueConnection(GameObject obj) {
        if (obj == null) {
            TrueConditionBlock = null;
            if (lineToTrue != null) {
                GameManager.instance.RemoveConnectLine(lineToTrue);
            }
            lineToTrue = null;

            return;
        }

        TrueConditionBlock = obj.GetComponent<FunctionBlock>();

        if (lineToTrue != null)
            GameManager.instance.RemoveConnectLine(lineToTrue);
        lineToTrue = GameManager.instance.CreateConnectPrimary(this, TrueConditionBlock, true);
    }

    private void SetFalseConnection(GameObject obj) {
        if (obj == null) {
            FalseConditionBlock = null;
            if (lineToFalse != null) {
                GameManager.instance.RemoveConnectLine(lineToFalse);
            }
            lineToFalse = null;

            return;
        }

        FalseConditionBlock = obj.GetComponent<FunctionBlock>();

        if (lineToFalse != null)
            GameManager.instance.RemoveConnectLine(lineToFalse);
        lineToFalse = GameManager.instance.CreateConnectSecondary(this, FalseConditionBlock, true);
    }
}