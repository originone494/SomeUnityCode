using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.BT
{
    public class BehaviorTreeBuilder
    {
        private readonly Stack<BtBehaviour> nodeStack;//构建树结构用的栈
        private readonly BehaviorTree bhTree;//构建的树

        public BehaviorTreeBuilder()
        {
            bhTree = new BehaviorTree(null);//构造一个没有根的树
            nodeStack = new Stack<BtBehaviour>();//初始化构建栈
        }

        public BehaviorTreeBuilder AddBehavior(BtBehaviour behavior)
        {
            if (bhTree.HaveRoot)//有根节点时，加入构建栈
            {
                nodeStack.Peek().AddChild(behavior);
            }
            else //当树没根时，新增得节点视为根节点
            {
                bhTree.SetRoot(behavior);
            }
            //只有组合节点和修饰节点需要进构建堆
            if (behavior is BtComposite || behavior is BtDecorator)
            {
                nodeStack.Push(behavior);
            }

            return this;
        }

        public void TreeTick()
        {
            bhTree.Tick();
        }

        public BehaviorTreeBuilder Back()
        {
            nodeStack.Pop();
            return this;
        }

        public BehaviorTree End()
        {
            nodeStack.Clear();
            return bhTree;
        }

        //---------包装各节点---------
        public BehaviorTreeBuilder Sequence()
        {
            var tp = new BtSequence();
            AddBehavior(tp);
            return this;
        }

        public BehaviorTreeBuilder Seletctor()
        {
            var tp = new BtSelector();
            AddBehavior(tp);
            return this;
        }

        public BehaviorTreeBuilder Filter()
        {
            var tp = new BtFilter();
            AddBehavior(tp);
            return this;
        }

        public BehaviorTreeBuilder Parallel(BtParallel.Policy success, BtParallel.Policy failure)
        {
            var tp = new BtParallel(success, failure);
            AddBehavior(tp);
            return this;
        }

        public BehaviorTreeBuilder Monitor(BtParallel.Policy success, BtParallel.Policy failure)
        {
            var tp = new BtMonitor(success, failure);
            AddBehavior(tp);
            return this;
        }

        public BehaviorTreeBuilder ActiveSelector()
        {
            var tp = new BtActiveSelector();
            AddBehavior(tp);
            return this;
        }

        public BehaviorTreeBuilder Repeat(int limit)
        {
            var tp = new BtRepeat(limit);
            AddBehavior(tp);
            return this;
        }

        public BehaviorTreeBuilder Inverter()
        {
            var tp = new BtInverter();
            AddBehavior(tp);
            return this;
        }

        public BehaviorTreeBuilder DebugNode(string word)
        {
            var node = new DebugNode(word);
            AddBehavior(node);
            return this;
        }

        public void OnDisable()
        {
            nodeStack.Clear();
        }
    }
}