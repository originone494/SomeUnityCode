using ARPG.Animation;
using UnityEngine;

namespace ARPG.FSM
{
    public class FallState : PlayerBaseState
    {
        private CharacterController cc;

        public FallState(CharacterController _cc, AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
            cc = _cc;
        }

        public override void Condition()
        {
            if (playerParam.OnGround)
            {
                fsmSystem.SetState(StateType.Walk);
            }
        }

        public override void OnEnter()
        {
            playerParam.RootMotion = false;
            playerParam.ApplyRotation = false;
        }

        public override void OnExit()
        {
            playerParam.RootMotion = true;
            playerParam.ApplyRotation = true;
        }

        public override void OnFixedUpdate()
        {
            playerParam.velocity.y -= playerParam.gravity * Time.fixedDeltaTime * 1.5f;
            playerParam.velocity.y = Mathf.Max(playerParam.velocity.y, -80f);

            playerParam.averageVel.y = playerParam.velocity.y;
            cc.Move(playerParam.averageVel * Time.fixedDeltaTime);
        }

        public override void OnLaterUpdate()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}