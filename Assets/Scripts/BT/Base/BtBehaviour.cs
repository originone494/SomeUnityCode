namespace ARPG.BT
{
    public enum EStatus
    {
        //失败，成功，运行中，中断，无效
        Failure, Success, Running, Aborted, Invalid
    }

    public abstract class BtBehaviour
    {
        public bool IsTerminated => IsSuccess || IsFailure;//是否运行结束
        public bool IsSuccess => status == EStatus.Success;//是否成功
        public bool IsFailure => status == EStatus.Failure;//是否失败
        public bool IsRunning => status == EStatus.Running;//是否正在运行

        protected EStatus status;//运行状态

        public BtBehaviour()
        {
            status = EStatus.Invalid;
        }

        protected virtual void OnInitialize()
        { }

        protected abstract EStatus OnUpdate();

        protected virtual void OnTerminate()
        { }

        public EStatus Tick()
        {
            if (!IsRunning)
                OnInitialize();
            status = OnUpdate();
            if (!IsRunning)
                OnTerminate();
            return status;
        }

        public virtual void AddChild(BtBehaviour child)
        { }

        public void Reset()
        {
            status = EStatus.Invalid;
        }

        public void Abort()
        {
            OnTerminate();
            status = EStatus.Aborted;
        }
    }
}