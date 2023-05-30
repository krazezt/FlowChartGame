using System;
using System.Collections.Generic;

[Serializable]
public class FunctionBlockDTO {
    public int BlockTypeID;
    public string Text;
    public List<int> ConnectBlocks;
    public List<int> NextBlocks;
}