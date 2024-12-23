using System.Collections.Generic;

namespace ARPG.BT
{
    public abstract class BtComposite : BtBehaviour
    {
        protected LinkedList<BtBehaviour> children;

        public BtComposite()
        {
            children = new LinkedList<BtBehaviour>();
        }

        //移除指定子节点
        public virtual void RemoveChild(BtBehaviour child)
        {
            children.Remove(child);
        }

        public void ClearChildren()//清空子节点列表
        {
            children.Clear();
        }

        public override void AddChild(BtBehaviour child)//添加子节点
        {
            //默认是尾插入，如：0插入「1，2，3」中，就会变成「1，2，3，0」
            children.AddLast(child);
        }
    }
}