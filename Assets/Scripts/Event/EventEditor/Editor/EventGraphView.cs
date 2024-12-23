using UnityEditor;
using UnityEngine;

namespace ARPG.EventNSystem
{
    public class EventGraphView
    {
        private EventGraphWindow m_Window;

        //根节点
        private static EventGroupNode m_Root;

        private int m_CurrentID;

        private EventGraph m_Graph;

        public EventGraphView(EventGraphWindow window, EventGraph graph)
        {
            m_Window = window;
            m_Graph = graph;

            LoadData(graph);
        }

        public void Draw()
        {
            EditorGUI.DrawRect(new Rect(0, 0, m_Window.position.width, m_Window.position.height)
                , new Color(0.1f, 0.1f, 0.1f, 0.8f));

            //绘制节点
            m_Root?.Draw();
        }

        public bool ClickOnNode(Vector2 pos, out BaseEventNode node)
        {
            if (m_Root != null)
            {
                return m_Root.ClickOn(pos, out node);
            }
            node = null;
            return false;
        }

        public void AddNode<T>(object obj) where T : BaseEventNode, new()
        {
            EventGroupNode node = (EventGroupNode)obj;
            T newNode = new T() { id = m_CurrentID++ };
            newNode.nodeRect.x = node.nodeRect.x + 300;
            newNode.nodeRect.y = node.nodeRect.y;
            newNode.parent = node;
            node.AddNode(newNode);
        }

        public void DeleteNode(object obj)
        {
            BaseEventNode node = (BaseEventNode)obj;
            node.DeleteNode();
        }

        public static bool IsEventNameRepeated(string name, int id)
        {
            return m_Root.IsNameRepeated(name, id);
        }

        public void Move(Vector2 delta)
        {
            m_Root.MoveNode(delta);

            m_Window.Repaint();
        }

        public void SaveData()
        {
            if (m_Graph.nodes != null)
            {
                m_Graph.nodes.Clear();
            }

            m_Root.CopyNode(m_Graph.nodes);
            m_Graph.currentID = m_CurrentID;

            EditorUtility.SetDirty(m_Graph);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void LoadData(EventGraph graph)
        {
            if (graph.nodes == null || graph.nodes.Count == 0)
            {
                m_Root = new EventGroupNode() { id = m_CurrentID++, eventName = "Root" };
            }
            else
            {
                m_Root = new EventGroupNode() { id = m_CurrentID++, eventName = "Root" };
                m_Root.LoadNode(graph.nodes, 0);
                m_CurrentID = m_Graph.currentID;
            }
        }
    }
}