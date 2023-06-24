using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class VariablesPanel : MonoBehaviour {
    private List<VariableBlock> variables;
    private List<VariableBlock> customableVariables;

    public enum State {
        Hiding,
        ShowingCustomables,
        ShowingAll
    }

    private RectTransform rectTransform;

    public State CurrentState;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetVariables(List<VariableBlock> variables, List<VariableBlock> customableVariables) {
        this.variables = variables;
        this.customableVariables = customableVariables;

        SetState(State.ShowingCustomables);
    }

    public void Hide() {
        SetState(State.ShowingCustomables);

        CurrentState = State.Hiding;
        gameObject.SetActive(false);
    }

    public void Show(State state) {
        SetState(state);
        gameObject.SetActive(true);
    }

    public void ToggleCustomable() {
        gameObject.SetActive(!gameObject.activeSelf);
        SetState(gameObject.activeSelf ? State.ShowingCustomables : State.Hiding);
    }

    public void ResetVariables() {
        foreach (var variable in variables) {
            variable.AssignOutputValue(0);
            variable.DisplayValue();
        }
    }

    private void SetState(State state) {
        CurrentState = state;
        switch (state) {
            case State.ShowingCustomables:
                rectTransform.sizeDelta = new(GameConfig.VARIABLE_PANEL_WIDTH, GameConfig.VARIABLE_PANEL_SEGMENT_HEIGHT * customableVariables.Count);
                rectTransform.anchoredPosition = new(0, -GameConfig.VARIABLE_PANEL_SEGMENT_HEIGHT * customableVariables.Count / 2);
                foreach (var item in variables) {
                    item.gameObject.SetActive(false);
                    item.SetInteractable(false);
                }
                foreach (var item in customableVariables) {
                    item.gameObject.SetActive(true);
                    item.SetInteractable(true);
                }
                break;

            case State.ShowingAll:
                rectTransform.sizeDelta = new(GameConfig.VARIABLE_PANEL_WIDTH, GameConfig.VARIABLE_PANEL_SEGMENT_HEIGHT * variables.Count);
                rectTransform.anchoredPosition = new(0, -GameConfig.VARIABLE_PANEL_SEGMENT_HEIGHT * variables.Count / 2);
                foreach (var item in variables) {
                    item.gameObject.SetActive(true);
                    item.SetInteractable(false);
                }
                break;
        }
    }
}