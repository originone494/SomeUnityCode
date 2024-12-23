using ARPG.Animation;

namespace ARPG.FSM
{
    public class DamageState : PlayerBaseState
    {
        public DamageState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
        }

        public override void Condition()
        {
            if (animSystem.IsAnimationComplete("Damage") >= 1f)
            {
                fsmSystem.SetState(StateType.Walk);
            }
        }

        public override void OnEnter()
        {
            playerParam.RootMotion = false;
        }

        public override void OnExit()
        {
            playerParam.RootMotion = true;
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnLaterUpdate()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}