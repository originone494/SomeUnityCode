using ARPG.BT;
using UnityEngine;

public class DebugNode : BtBehaviour
{
    private string word;

    public DebugNode(string word)
    {
        this.word = word;
    }

    protected override EStatus OnUpdate()
    {
        Debug.Log(word);
        return EStatus.Success;
    }
}