using UnityEngine;

public class StartBlock : ActionBlock {

    public override bool ExecuteFunction() {
        return base.ExecuteFunction();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            ExecuteFunction();
    }
}