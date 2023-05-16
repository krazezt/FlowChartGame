public class EndBlock : ActionBlock {

    public override bool ExecuteFunction() {
        GameManager.instance.AppendResultLinePoint(gameObject);

        if (CheckEndCondition()) {
            return true;
        } else
            return false;
    }

    protected virtual bool CheckEndCondition() {
        return false;
    }
}