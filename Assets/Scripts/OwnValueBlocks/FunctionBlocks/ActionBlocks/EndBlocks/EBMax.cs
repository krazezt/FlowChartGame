using System.Collections.Generic;

public class EBMax : EndBlock {
    protected override bool CheckEndCondition() {
        int max = 0;

        foreach (var item in inputValues) {
            if (max == 0)
                max = item.GetOutputValue();

            if (item.GetOutputValue() > max)
                max = item.GetOutputValue();
        }

        return checkValue.GetOutputValue() == max;
    }
}