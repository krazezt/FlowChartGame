using UnityEngine;

public class StartBlock : ActionBlock {

    public override void ExecuteFunction() {
        GameManager.instance.ClearResultPoint();

        base.ExecuteFunction();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            ExecuteFunction();
    }
}