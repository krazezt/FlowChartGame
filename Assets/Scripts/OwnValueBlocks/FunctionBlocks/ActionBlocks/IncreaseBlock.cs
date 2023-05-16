public class IncreaseBlock : ActionBlock {
    public VariableBlock VariableBlock;

    public override bool ExecuteFunction() {
        OutputValue = VariableBlock.GetOutputValue() + 1;
        VariableBlock.AssignOutputValue(OutputValue);

        return base.ExecuteFunction();
    }
}