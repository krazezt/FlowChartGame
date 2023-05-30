using System;
using System.Collections.Generic;

[Serializable]
public class TestCaseDTO {
    public string Text;
    public int TestCaseTypeID;
    public List<int> VariableIDs;
}