using System;
using UnityEngine;

public class EBUCLN : EndBlock {

    protected override bool CheckEndCondition() {
        int i;
        for (i = Math.Min(inputValues[0].GetOutputValue(), inputValues[1].GetOutputValue()); i >= 1; i--) {
            Debug.Log("Here???");

            if (inputValues[0].GetOutputValue() % i == 0 && inputValues[1].GetOutputValue() % i == 0) {
                Debug.Log("Here???");
                Debug.Log("End values:" + checkValue.GetOutputValue() + ", " + i);
                return checkValue.GetOutputValue() == i;
            }
        }

        Debug.Log("End values:" + checkValue.GetOutputValue() + ", " + i);
        return checkValue.GetOutputValue() == i;
    }
}