using System.Collections.Generic;

public class EndBlock : ActionBlock {

    // List of values that will be used to calculate the "requiredValue"
    public List<OwnValueBlock> inputValues;

    // The value to be check if it's value is equals to the "requiredValue"
    public OwnValueBlock checkValue;

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