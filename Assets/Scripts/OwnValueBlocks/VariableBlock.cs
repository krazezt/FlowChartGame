using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VariableBlock : OwnValueBlock {
    public string variableName;
    public TMP_Text valueText;

    private Button button;

    public void SetInteractable(bool interactable) {
        if (button == null) {
            button = GetComponent<Button>();
        }

        button.interactable = interactable;
    }

    public void SetValueText(string value) {
        try {
            valueText.text = int.Parse(value).ToString();
        } catch {
            valueText.text = 0.ToString();
        }

        GameManager.instance.UpdateCustomCasePairValues();
    }

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