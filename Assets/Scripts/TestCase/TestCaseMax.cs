using UnityEngine;

public class TestCaseMax : TestCase {

    [Header("Values")]
    public int a;
    public int b;
    public VariableBlock variableA;
    public VariableBlock variableB;

    public override void SetupTestCase() {
        variableA.AssignOutputValue(a);
        variableB.AssignOutputValue(b);
    }
}