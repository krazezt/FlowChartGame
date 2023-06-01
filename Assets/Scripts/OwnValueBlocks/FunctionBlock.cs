using UnityEngine;

public class FunctionBlock : OwnValueBlock {
    public virtual bool ExecuteFunction() {
        // Debug.Log("FunctionBlock: " + gameObject.name);
        GameManager.instance.AppendResultLinePoint(gameObject);
        return false;
    }
}