using System.Collections.Generic;

public class EBMax : EndBlock {
    public OwnValueBlock checkValue;
    public List<OwnValueBlock> listValue;

    protected override bool CheckEndCondition() {
        int max = 0;

        foreach (var item in listValue) {
            if (max == 0)
                max = item.GetOutputValue();

            if (item.GetOutputValue() > max)
                max = item.GetOutputValue();
        }

        return checkValue.GetOutputValue() == max;
    }
}