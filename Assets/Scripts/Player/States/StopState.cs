using ARPG.Animation;
using System.Runtime.InteropServices.ComTypes;

namespace ARPG.FSM
{
    public class StopState : PlayerBaseState
    {
        private bool isTurn;

        public StopState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
        }

        public override void Condition()
        {
            if (!isTurn)
            {
                if (animSystem.IsAnimationComplete("RunStop") >= 0.99f)
                {
                    fsmSystem.SetState(StateType.Walk);
                }
            }
        }

        public override void OnEnter()
        {
            playerParam.ApplyRotation = false;

            isTurn = false;
        }

        public override void OnExit()
        {
            playerParam.ApplyRotation = true;
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