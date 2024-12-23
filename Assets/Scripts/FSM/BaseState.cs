using ARPG.Animation;

namespace ARPG.FSM
{
    public abstract class BaseState
    {
        protected AnimSystem animSystem;

        protected BaseState(AnimSystem _animSystem)
        {
            animSystem = _animSystem;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnFixedUpdate();

        public abstract void OnLaterUpdate();

        public abstract void OnExit();

        public abstract void Condition();
    }
}