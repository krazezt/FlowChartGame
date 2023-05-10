public class AssignBlock : ActionBlock {
    public VariableBlock VariableBlock;
    public OwnValueBlock ValueBlock;

    public override void ExecuteFunction() {
        OutputValue = ValueBlock.GetOutputValue();
        VariableBlock.AssignOutputValue(OutputValue);

        base.ExecuteFunction();
    }
}