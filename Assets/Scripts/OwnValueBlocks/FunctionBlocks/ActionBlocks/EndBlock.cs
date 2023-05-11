using UnityEngine;

public class EndBlock : ActionBlock {

    public override void ExecuteFunction() {
        Debug.Log("EndBlock: " + gameObject.name);
        GameManager.instance.AppendResultLinePoint(gameObject);

        if (CheckEndCondition()) {
            Debug.Log("Win");
        }
    }

    protected virtual bool CheckEndCondition() {
        return false;
    }
}