public class OperationBlock : ActionBlock {

    public enum Operator {
        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE,
    }

    public OwnValueBlock LeftSideOperandBlock;
    public OwnValueBlock RightSideOperandBlock;
    public Operator operatorType;

    public override void ExecuteFunction() {
        switch (operatorType) {
            case Operator.ADD:
                OutputValue = LeftSideOperandBlock.GetOutputValue() + RightSideOperandBlock.GetOutputValue();
                break;

            case Operator.SUBTRACT:
                OutputValue = LeftSideOperandBlock.GetOutputValue() - RightSideOperandBlock.GetOutputValue();
                break;

            case Operator.MULTIPLY:
                OutputValue = LeftSideOperandBlock.GetOutputValue() * RightSideOperandBlock.GetOutputValue();
                break;

            case Operator.DIVIDE:
                OutputValue = LeftSideOperandBlock.GetOutputValue() / RightSideOperandBlock.GetOutputValue();
                break;
        }

        base.ExecuteFunction();
    }
}