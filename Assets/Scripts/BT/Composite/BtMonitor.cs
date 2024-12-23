namespace ARPG.BT
{
    public class BtMonitor : BtParallel
    {
        public BtMonitor(Policy mSuccessPolicy, Policy mFailurePolicy)
     : base(mSuccessPolicy, mFailurePolicy)
        {
        }

        public void AddCondition(BtBehaviour condition)
        {
            children.AddFirst(condition);
        }

        public void AddAction(BtBehaviour action)
        {
            children.AddLast(action);
        }
    }
}