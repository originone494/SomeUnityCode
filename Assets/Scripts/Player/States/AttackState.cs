using ARPG.Animation;
using UnityEngine;

namespace ARPG.FSM
{
    public class AttackState : PlayerBaseState
    {
        private float phase;
        private Quaternion targetRotation;
        private bool isTurn;
        private bool isAttacking;

        private float distance;

        public AttackState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
            phase = -1;
        }

        public override void Condition()
        {
            distance = 100f;

            if (animSystem.IsAnimationComplete("LAtk_" + phase.ToString("f0")) >= 0.99f)
            {
                fsmSystem.SetState(StateType.Walk);
            }
            else
            {
                if (playerParam.isEndAttacking)
                {
                    isAttacking = false;

                    if (inputSystem.playerLAtk)
                    {
                        playerParam.isEndAttacking = false;

                        if (phase != 4) phase++;

                        animSystem.TransitionTo("LAtk_" + phase.ToString("f0"));
                        isAttacking = true;

                        Collider[] colliders = Physics.OverlapSphere(playerParam.PlayerModel.position, 1.5f, LayerMask.GetMask("Enemy"));

                        if (colliders.Length > 0)
                        {
                            Vector3 targetDir = colliders[0].transform.position - playerParam.PlayerModel.position;
                            targetDir.y = 0f;
                            targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
                            isTurn = true;

                            distance = Vector3.Distance(playerParam.PlayerModel.position, colliders[0].transform.position);
                        }
                    }
                    else if (inputSystem.playerDodge)//翻滚
                    {
                        fsmSystem.SetState(StateType.Dodge);
                    }
                    else if (inputSystem.playerSkill && playerParam.isWeapon)//技能
                    {
                        fsmSystem.SetState(StateType.Skill);
                    }
                    else if (phase == 4 && animSystem.IsAnimationComplete("LAtk_" + phase.ToString("f0")) >= 0.3f && inputSystem.playerMovement != Vector2.zero)
                    {
                        fsmSystem.SetState(StateType.Walk);
                    }
                }
            }
            if (inputSystem.playerSkill)
            {
                fsmSystem.SetState(StateType.Skill, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.K))//受到伤害
            {
                fsmSystem.SetState(StateType.Damage);
            }
            else if (inputSystem.playerDefend)
            {
                fsmSystem.SetState(StateType.Defend);
            }

            //Debug.Log(isTurn + " " + isAttacking);
        }

        public override void OnEnter()
        {
            if (phase < 0)
            {
                phase = 1;
            }
            playerParam.ApplyRotation = false;
            isTurn = false;
            isAttacking = true;
            playerParam.hitCount = 0;
        }

        public override void OnExit()
        {
            phase = -1;
            playerParam.RootMotion = true;
            playerParam.ApplyRotation = true;
        }

        public override void OnFixedUpdate()
        {
            if (isTurn)
            {
                playerParam.PlayerModel.rotation = Quaternion.Lerp(playerParam.PlayerModel.rotation, targetRotation, 0.5f);

                if (Quaternion.Angle(playerParam.PlayerModel.rotation, targetRotation) < 0.1f)
                {
                    isTurn = false;
                }
            }
        }

        public override void OnLaterUpdate()
        {
        }

        public override void OnUpdate()
        {
            if (isAttacking)
            {
                playerParam.ApplyRotation = false;
            }
            else
            {
                if (isTurn)
                {
                    playerParam.ApplyRotation = false;
                }
                else
                {
                    playerParam.ApplyRotation = true;
                }
            }

            Collider[] colliders = Physics.OverlapSphere(playerParam.PlayerModel.position, 1.5f, LayerMask.GetMask("Enemy"));

            if (colliders.Length > 0)
            {
                distance = Vector3.Distance(playerParam.PlayerModel.position, colliders[0].transform.position);
            }
            if (distance < 1.2f)
            {
                playerParam.RootMotion = false;
            }
            else
            {
                playerParam.RootMotion = true;
            }
        }
    }
}