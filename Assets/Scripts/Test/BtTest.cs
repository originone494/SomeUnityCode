using ARPG.BT;
using UnityEngine;

public class BtTest : MonoBehaviour
{
    private BehaviorTreeBuilder builder;

    private void Awake()
    {
        builder = new BehaviorTreeBuilder();
    }

    private void Start()
    {
        builder.Repeat(3)
                    .Sequence()
                        .DebugNode("Ok,")//由于动作节点不进栈，所以不用Back
                        .DebugNode("It's ")
                        .DebugNode("My time")
                    .Back()
                .End();
        builder.TreeTick();
    }
}