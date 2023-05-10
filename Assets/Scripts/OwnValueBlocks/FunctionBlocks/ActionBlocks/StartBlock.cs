using UnityEngine;

public class StartBlock : ActionBlock {

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            ExecuteFunction();
    }
}