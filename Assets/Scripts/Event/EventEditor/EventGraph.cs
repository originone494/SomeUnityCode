using System.Collections.Generic;
using UnityEngine;

namespace ARPG.EventNSystem
{
    [CreateAssetMenu(fileName = "New Event Graph", menuName = "New Event Setting/Event Graph")]
    public class EventGraph : ScriptableObject
    {
        public List<SerializableNode> nodes = new List<SerializableNode>();
        public int currentID;
    }
}