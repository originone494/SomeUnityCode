using System.Collections.Generic;
using UnityEngine;

namespace ARPG.EventNSystem
{
    public abstract class BaseEventNode
    {
        public Rect nodeRect = new Rect(10, 10, 250, 100);

        public string nodeTitle;

        public string eventName;

        public int id;

        public bool legal;

        public Color m_TextColor;

        public EventGroupNode parent;

        public virtual void Draw()
        {
            GUIStyle style = new GUIStyle(GUI.skin.window);

            style.normal.textColor = m_TextColor;

            nodeRect = GUI.Window(id, nodeRect, NodeCallBack, nodeTitle, style);
        }

        protected virtual void NodeCallBack(int id)
        {
            GUI.Label(new Rect(15, 25, 80, 20), "Name");
            eventName = GUI.TextField(new Rect(70, 25, 160, 20), eventName);

            legal = false;
            if (string.IsNullOrEmpty(eventName))
            {
                GUI.Label(new Rect(15, 55, 200, 20), "Please Input Event Name");
            }
            else if (EventGraphView.IsEventNameRepeated(eventName, id))
            {
                GUI.Label(new Rect(15, 55, 200, 20), "Event Name Is Repeate");
            }
            else
            {
                legal = true;
            }

            GUI.DragWindow();
        }

        public virtual bool ClickOn(Vector2 pos, out BaseEventNode node)
        {
            if (nodeRect.Contains(pos))
            {
                node = this;
                return true;
            }
            node = null;
            return false;
        }

        public virtual void DeleteNode()
        {
            if (parent != null)
            {
                parent.DeleteNode(this);
            }
        }

        public virtual bool IsNameRepeated(string name, int id)
        {
            if (eventName == name && id != this.id)
            {
                return true;
            }
            return false;
        }

        public virtual void MoveNode(Vector2 delta)
        {
            nodeRect.position += delta;
        }

        public abstract void CopyNode(List<SerializableNode> container);

        public virtual void LoadNode(List<SerializableNode> container, int id)
        {
            var data = container.Find(n => n.id == id);
            if (data != null)
            {
                eventName = data.name;
                this.id = data.id;
                nodeRect = data.rect;
            }
        }
    }
}