using ARPG.Animation;
using UnityEngine;

namespace ARPG.FSM
{
    public class SkillState : PlayerBaseState
    {
        private Quaternion targetRotation;
        private bool isTurn;

        public SkillState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
        }

        public override void Condition()
        {
            //Debug.Log(animSystem.IsAnimationComplete("Skill"));
            if (animSystem.IsAnimationComplete("Skill") >= 0.99f)
            {
                fsmSystem.SetState(StateType.Walk);
            }
            else
            {
                if (animSystem.IsAnimationComplete("Skill") >= 0.6f)
                {
                    if (inputSystem.playerMovement != UnityEngine.Vector2.zero)
                    {
                        fsmSystem.SetState(StateType.Walk);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            playerParam.ApplyRotation = false;
            Collider[] colliders = Physics.OverlapSphere(playerParam.PlayerModel.position, 3.5f, LayerMask.GetMask("Enemy"));

            if (colliders.Length > 0)
            {
                Vector3 targetDir = colliders[0].transform.position - playerParam.PlayerModel.position;
                targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);

                isTurn = true;
            }
        }

        public override void OnExit()
        {
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
        }
    }
}