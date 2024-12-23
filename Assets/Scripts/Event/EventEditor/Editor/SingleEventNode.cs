using System.Collections.Generic;
using UnityEngine;

namespace ARPG.EventNSystem
{
    public class SingleEventNode : BaseEventNode
    {
        public SingleEventNode()
        {
            nodeTitle = "Event";
            m_TextColor = Color.cyan;
        }

        public override void CopyNode(List<SerializableNode> container)
        {
            if (!legal) return;

            SerializableNode snode = new SerializableNode();
            snode.type = "Single";
            snode.name = eventName;
            snode.id = id;
            snode.rect = nodeRect;
            container.Add(snode);
        }
    }
}