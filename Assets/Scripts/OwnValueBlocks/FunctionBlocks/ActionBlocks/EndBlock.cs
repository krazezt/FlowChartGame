using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EndBlock : ActionBlock {
    public static int requestingCount;
    public static bool isAllConditionTrue;

    // List of values that will be used to calculate the "requiredValue"
    public List<OwnValueBlock> inputValues;

    // The value to be check if it's value is equals to the "requiredValue"
    public OwnValueBlock checkValue;

    // Content in the CheckEndCondition function, will be run in the EndBlock. Type & Params:
    // (List<OwnValueBlock> list, OwnValueBlock checkValue) => bool
    public string validateURL;

    public override bool ExecuteFunction() {
        GameManager.instance.AppendResultLinePoint(gameObject);
        StartCoroutine(RequestValidate());

        if (CheckEndCondition()) {
            return true;
        } else
            return false;
    }

    private IEnumerator RequestValidate() {
        requestingCount++;
        WWWForm form = new();

        int i;
        for (i = 1; i <= inputValues.Count; i++)
            form.AddField("key" + i.ToString(), inputValues[i - 1].GetOutputValue());
        form.AddField("key" + i.ToString(), checkValue.GetOutputValue());

        using UnityWebRequest www = UnityWebRequest.Post(validateURL, form);
        yield return www.SendWebRequest();

        requestingCount--;
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        } else {
            if (www.downloadHandler.text == "false")
                isAllConditionTrue = false;
            Debug.Log("Request success: " + www.downloadHandler.text);
        }
    }

    protected virtual bool CheckEndCondition() {
        return true;
    }
}