using TMPro;

public class VariableBlock : OwnValueBlock {
    public string variableName;
    public TMP_Text valueText;

    public void AssignOutputValue(int value) {
        OutputValue = value;
    }

    public virtual void DisplayValue() {
        valueText.text = OutputValue.ToString();
    }

    public virtual void DisplayValue(int value) {
        valueText.text = value.ToString();
    }
}