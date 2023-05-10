public class AssignBlock : ActionBlock {
    public VariableBlock VariableBlock;

    public override void ExecuteFunction() {
        OutputValue = VariableBlock.GetOutputValue();
        VariableBlock.AssignOutputValue(OutputValue);

        base.ExecuteFunction();
    }
}