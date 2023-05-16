using UnityEngine;

public class StartBlock : ActionBlock {

    public override bool ExecuteFunction() {
        GameManager.instance.ClearResultPoint();

        return base.ExecuteFunction();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            ExecuteFunction();
    }
}