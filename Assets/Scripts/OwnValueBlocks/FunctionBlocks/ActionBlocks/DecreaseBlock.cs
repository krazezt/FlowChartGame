public class DecreaseBlock : ActionBlock {
    public VariableBlock VariableBlock;

    public override void ExecuteFunction() {
        OutputValue = VariableBlock.GetOutputValue() - 1;
        VariableBlock.AssignOutputValue(OutputValue);

        base.ExecuteFunction();
    }
}