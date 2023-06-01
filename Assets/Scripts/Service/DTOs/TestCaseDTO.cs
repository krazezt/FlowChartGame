using System;
using System.Collections.Generic;

[Serializable]
public class TestCaseDTO {
    public int testCaseTypeID;
    public string text;
    public List<CasePairDTO> casePairs;
}