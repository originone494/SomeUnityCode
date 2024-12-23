namespace ARPG.BT
{
    public abstract class BtDecorator : BtBehaviour
    {
        protected BtBehaviour child;

        public override void AddChild(BtBehaviour child)
        {
            this.child = child;
        }
    }
}