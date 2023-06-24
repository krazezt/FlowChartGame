using System;

public class EBUCLN : EndBlock {

    protected override bool CheckEndCondition() {
        int i;
        for (i = Math.Min(inputValues[0].GetOutputValue(), inputValues[1].GetOutputValue()); i >= 1; i--)
            if (inputValues[0].GetOutputValue() % i == 0 && inputValues[1].GetOutputValue() % i == 0)
                return checkValue.GetOutputValue() == i;
        return checkValue.GetOutputValue() == i;
    }
}