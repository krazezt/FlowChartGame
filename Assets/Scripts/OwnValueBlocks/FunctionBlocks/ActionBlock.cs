public class ActionBlock : FunctionBlock {
    public FunctionBlock NextBlock;

    public override void ExecuteFunction() {
        base.ExecuteFunction();

        NextBlock.ExecuteFunction();
    }
}