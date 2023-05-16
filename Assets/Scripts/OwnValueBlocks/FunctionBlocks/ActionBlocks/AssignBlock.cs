public class AssignBlock : ActionBlock {
    public VariableBlock VariableBlock;
    public OwnValueBlock ValueBlock;

    public override bool ExecuteFunction() {
        OutputValue = ValueBlock.GetOutputValue();
        VariableBlock.AssignOutputValue(OutputValue);

        return base.ExecuteFunction();
    }
}