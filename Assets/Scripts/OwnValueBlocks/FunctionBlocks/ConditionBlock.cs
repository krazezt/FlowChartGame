public class ConditionBlock : FunctionBlock {

    public enum Operator {
        LESS,
        GREATER,
        LESS_OR_EQUAL,
        GREATER_OR_EQUAL,
    }

    public FunctionBlock TrueConditionBlock;
    public FunctionBlock FalseConditionBlock;

    public OwnValueBlock LeftSideOperandBlock;
    public OwnValueBlock RightSideOperandBlock;
    public Operator operatorType;

    public override void ExecuteFunction() {
        base.ExecuteFunction();

        if (CheckCondition())
            TrueConditionBlock.ExecuteFunction();
        else
            FalseConditionBlock.ExecuteFunction();
    }

    protected bool CheckCondition() {
        switch (operatorType) {
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
}