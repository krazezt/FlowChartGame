using System;
using System.Collections.Generic;
using UnityEngine;
using Z.Expressions;

public class EndBlock : ActionBlock {

    // List of values that will be used to calculate the "requiredValue"
    public List<OwnValueBlock> inputValues;

    // The value to be check if it's value is equals to the "requiredValue"
    public OwnValueBlock checkValue;

    // Content in the CheckEndCondition function, will be run in the EndBlock. Type & Params:
    // (List<OwnValueBlock> list, OwnValueBlock checkValue) => bool
    public string validateCode;

    public override bool ExecuteFunction() {
        GameManager.instance.AppendResultLinePoint(gameObject);

        if (CheckEndCondition()) {
            return true;
        } else
            return false;
    }

    protected virtual bool CheckEndCondition() {
        return Eval.Execute<bool>(validateCode, new {
            inputValues,
            checkValue,
        });
    }
}