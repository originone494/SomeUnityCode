using System.Collections.Generic;
using Unity.VisualScripting;

namespace ARPG.BT
{
    public class BtFilter : BtSequence
    {
        public BtFilter(BtBehaviour condition)
        {
            AddCondition(condition);
        }

        public BtFilter(BtBehaviour condition1, BtBehaviour condition2)
        {
            AddCondition(condition1);
            AddCondition(condition2);
        }

        public BtFilter()
        {
        }

        public void AddCondition(BtBehaviour condition)//添加条件，就用头插入
        {
            children.AddFirst(condition);
        }

        public void AddAction(BtBehaviour action)//添加动作，就用尾插入
        {
            children.AddLast(action);
        }
    }
}