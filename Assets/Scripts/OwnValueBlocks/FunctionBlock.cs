using UnityEngine;

public class FunctionBlock : OwnValueBlock {
    public virtual bool ExecuteFunction() {
#if UNITY_EDITOR
        //Debug.Log("FunctionBlock: " + gameObject.name);
#endif
        return GameManager.instance.AppendResultLinePoint(gameObject);
    }
}