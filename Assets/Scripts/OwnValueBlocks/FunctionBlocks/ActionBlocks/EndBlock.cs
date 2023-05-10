using UnityEngine;

public class EndBlock : ActionBlock {

    public override void ExecuteFunction() {
        base.ExecuteFunction();

        if (CheckEndCondition()) {
            Debug.Log("Win");
        }
    }

    protected virtual bool CheckEndCondition() {
        return false;
    }
}