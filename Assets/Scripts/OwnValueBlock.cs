using UnityEngine;

public class OwnValueBlock : CoreBlock {

    [SerializeField]
    protected int OutputValue;

    public int GetOutputValue() {
        return OutputValue;
    }
}