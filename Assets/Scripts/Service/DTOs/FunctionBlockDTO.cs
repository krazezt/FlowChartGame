using System;
using System.Collections.Generic;

[Serializable]
public class FunctionBlockDTO {
    public int blockTypeID;                 // GameConfig.FunctionBlockType.
    public string text;
    public List<int> connectBlocks;         // Index in GameManager.ownValueBlocks.
}