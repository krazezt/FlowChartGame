using System;
using System.Collections.Generic;

[Serializable]
public class LevelDataDTO {
    public List<int> Variables;
    public List<FunctionBlockDTO> FunctionBlocks;
    public List<TestCaseDTO> TestCases;
}