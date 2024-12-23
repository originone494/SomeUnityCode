using ARPG.Animation;
using UnityEngine;

namespace ARPG.FSM
{
    public class IdleState : PlayerBaseState
    {
        public IdleState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
        }

        public override void Condition()
        {
            if (inputSystem.playerJump)
            {
                fsmSystem.SetState(StateType.Jump);
            }
            else if (inputSystem.playerDefend && playerParam.isWeapon)//防御
            {
                fsmSystem.SetState(StateType.Defend);
            }
            else if (inputSystem.playerDodge)
            {
                fsmSystem.SetState(StateType.Dodge);
            }
            else if (inputSystem.playerSkill && playerParam.isWeapon)
            {
                fsmSystem.SetState(StateType.Skill);
            }
            else if (inputSystem.playerSwitchSword)
            {
                fsmSystem.SetState(StateType.SwitchWeapon);
            }
            else if (inputSystem.playerMovement.magnitude != 0)
            {
                fsmSystem.SetState(StateType.Walk);
            }
        }

        public override void OnEnter()
        {
            playerParam.RootMotion = false;
            playerParam.ApplyRotation = false;
            inputSystem.playerRun = false;
        }

        public override void OnExit()
        {
            playerParam.RootMotion = true;
            playerParam.ApplyRotation = true;
            inputSystem.playerRun = false;
        }

        public override void OnFixedUpdate()
        {
            playerParam.velocity.y -= playerParam.gravity * Time.fixedDeltaTime;
            if (playerParam.OnGround && playerParam.velocity.y < -5f)
            {
                playerParam.velocity.y = -5f;
            }
            playerParam.cc.Move(playerParam.velocity * Time.fixedDeltaTime);
        }

        public override void OnLaterUpdate()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}