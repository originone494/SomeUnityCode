using ARPG.Animation;
using UnityEngine;

namespace ARPG.FSM
{
    public class JumpState : PlayerBaseState
    {
        private bool isPeak;
        private CharacterController cc;

        public JumpState(CharacterController _cc, AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
            cc = _cc;
        }

        public override void Condition()
        {
            if (playerParam.velocity.y >= playerParam.jumpMaxSpeed)
            {
                playerParam.velocity.y = playerParam.jumpMaxSpeed;
                isPeak = true;
            }

            if (isPeak && playerParam.velocity.y < 0)
            {
                fsmSystem.SetState(StateType.Fall);
            }
        }

        public override void OnEnter()
        {
            isPeak = false;
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
            if (!isPeak)
            {
                playerParam.velocity.y += playerParam.jumpForce * Time.fixedDeltaTime;
            }
            else
            {
                playerParam.velocity.y -= playerParam.gravity * Time.fixedDeltaTime;
            }

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