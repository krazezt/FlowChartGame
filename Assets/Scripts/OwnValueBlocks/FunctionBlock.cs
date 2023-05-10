using UnityEngine;

public class FunctionBlock : OwnValueBlock {
    public FunctionBlock PrevBlock;

    public virtual void ExecuteFunction() {
        Debug.Log("FunctionBlock: " + gameObject.name);
    }
}