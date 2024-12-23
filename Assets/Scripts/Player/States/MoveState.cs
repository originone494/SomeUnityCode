using ARPG.Animation;
using UnityEngine;

namespace ARPG.FSM
{
    public class MoveState : PlayerBaseState
    {
        private float counter;

        public MoveState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
        }

        public override void Condition()
        {
            if (inputSystem.playerJump)//跳跃
            {
                fsmSystem.SetState(StateType.Jump);
                //Debug.Log("jump");
            }
            else if (inputSystem.playerDefend && playerParam.isWeapon)//防御
            {
                fsmSystem.SetState(StateType.Defend);
            }
            else if (inputSystem.playerLAtk && playerParam.isWeapon)//攻击
            {
                fsmSystem.SetState(StateType.Attack);
            }
            else if (inputSystem.playerDodge)//翻滚
            {
                fsmSystem.SetState(StateType.Dodge);
            }
            else if (inputSystem.playerSkill && playerParam.isWeapon)//技能
            {
                fsmSystem.SetState(StateType.Skill);
            }
            else if (Input.GetKeyDown(KeyCode.K))//受到伤害
            {
                fsmSystem.SetState(StateType.Damage);
            }
            else if (inputSystem.playerSwitchSword)//切换武器
            {
                fsmSystem.SetState(StateType.SwitchWeapon);
            }
            else if (inputSystem.playerMovement == Vector2.zero && playerParam.averageVel.magnitude > 0f && inputSystem.playerRun)//急停
            {
                fsmSystem.SetState(StateType.Stop);
            }

            if (counter >= playerParam.enterIdleTime && !playerParam.isWeapon)//待机
            {
                fsmSystem.SetState(StateType.Idle, 0.5f);
            }
        }

        public override void OnEnter()
        {
            counter = 0f;
            if (playerParam.isWeapon)
            {
                animSystem.TransitionTo("SwordMove");
                animSystem.BlendTree1DSetAnimSpeed("SwordMove", 1.25f);
            }
            else
            {
                animSystem.TransitionTo("Move");
            }
        }

        public override void OnExit()
        {
            if (playerParam.isWeapon)
            {
                animSystem.BlendTree1DSetAnimSpeed("SwordMove", 1f);
            }
        }

        public override void OnFixedUpdate()
        {
            playerParam.velocity.y -= playerParam.gravity * Time.fixedDeltaTime;
            if (playerParam.OnGround && playerParam.velocity.y < -5f)
            {
                playerParam.velocity.y = -5f;
            }

            if (playerParam.isWeapon)
            {
                if (inputSystem.playerRun)
                {
                    animSystem.BlendTree1DSetAnimSpeed("SwordMove", 1.1f);
                    animSystem.BlendTree1DSetValue("SwordMove", inputSystem.playerMovement.magnitude * playerParam.runSpeed);
                }
                else
                {
                    animSystem.BlendTree1DSetAnimSpeed("SwordMove", 1.25f);
                    animSystem.BlendTree1DSetValue("SwordMove", inputSystem.playerMovement.magnitude * playerParam.walkSpeed);
                }
            }
            else
            {
                if (inputSystem.playerRun)
                {
                    animSystem.BlendTree1DSetValue("Move", inputSystem.playerMovement.magnitude * playerParam.runSpeed);
                }
                else
                {
                    animSystem.BlendTree1DSetValue("Move", inputSystem.playerMovement.magnitude * playerParam.walkSpeed);
                }
            }
        }

        public override void OnLaterUpdate()
        {
        }

        public override void OnUpdate()
        {
            counter += Time.deltaTime;
            if (inputSystem.playerMovement != Vector2.zero)
            {
                counter = 0f;
            }
            else if (inputSystem.playerMovement == Vector2.zero)
            {
                inputSystem.playerRun = false;
            }
            if (playerParam.isWeapon)
            {
                playerParam.moveMaxWeight = animSystem.BlendTree1DGetMaxWeightAnim("SwordMove");
            }
            else
            {
                playerParam.moveMaxWeight = animSystem.BlendTree1DGetMaxWeightAnim("Move");
            }
        }
    }
}