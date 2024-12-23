using ARPG.Animation;

namespace ARPG.FSM
{
    public class DodgeState : PlayerBaseState
    {
        private bool isFront;

        public DodgeState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
        }

        public override void Condition()
        {
            if (isFront)
            {
                if (animSystem.IsAnimationComplete("Dodge_F") >= 1f)
                {
                    fsmSystem.SetState(StateType.Walk, 0.3f);
                }
            }
            else
            {
                if (animSystem.IsAnimationComplete("Dodge_B") >= 1f)
                {
                    fsmSystem.SetState(StateType.Walk);
                }
            }
        }

        public override void OnEnter()
        {
            if (inputSystem.playerMovement.magnitude > 0.5f)
            {
                isFront = true;
            }
            else
            {
                isFront = false;
            }
        }

        public override void OnExit()
        {
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