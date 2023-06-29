using System;
using System.Collections.Generic;

[Serializable]
public class LevelDataDTO {
    public List<VariableDTO> variables;                     // GameConfig.VariableType
    public List<FunctionBlockDTO> functionBlocks;   // StartBlock, EndBlock included
    public List<TestCaseDTO> testCases;

    // Content in the CheckEndCondition function, will be run in the EndBlock.
    // Type & Params: (List<OwnValueBlock> inputValues, OwnValueBlock checkValue) => bool
    public string validateURL;
}