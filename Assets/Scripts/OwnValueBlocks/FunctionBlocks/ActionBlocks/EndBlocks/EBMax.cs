public class EBMax : EndBlock {

    protected override bool CheckEndCondition() {
        return true;
        //int max = 0;

        //foreach (var item in inputValues) {
        //    if (max == 0)
        //        max = item.GetOutputValue();

        //    if (item.GetOutputValue() > max)
        //        max = item.GetOutputValue();
        //}

        //Debug.Log(checkValue.GetOutputValue() == max);
        //return checkValue.GetOutputValue() == max;
    }
}