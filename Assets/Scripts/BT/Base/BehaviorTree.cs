namespace ARPG.BT
{
    public class BehaviorTree
    {
        public bool HaveRoot => root != null;
        private BtBehaviour root;//根节点

        public BehaviorTree(BtBehaviour root)
        {
            this.root = root;
        }

        public void Tick()
        {
            root.Tick();
        }

        public void SetRoot(BtBehaviour root)
        {
            this.root = root;
        }
    }
}