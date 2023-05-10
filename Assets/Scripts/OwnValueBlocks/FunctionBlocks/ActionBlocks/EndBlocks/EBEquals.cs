using UnityEngine;

public class EBEquals : EndBlock {
    public OwnValueBlock RequiredValueBlock;
    public OwnValueBlock ValueBlock;

    protected override bool CheckEndCondition() {
        Debug.Log("End: Require = " + RequiredValueBlock.GetOutputValue() + ", Value = " + ValueBlock.GetOutputValue());
        return RequiredValueBlock.GetOutputValue() == ValueBlock.GetOutputValue();
    }
}