using ARPG.Animation;
using UnityEngine;

namespace ARPG.FSM
{
    public class SwitchWeaponState : PlayerBaseState
    {
        private bool isGetting;
        private float time;

        public SwitchWeaponState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
        }

        public override void Condition()
        {
            //如果佩戴武器
            if (isGetting)
            {
                if (animSystem.IsAnimationComplete("HoldSword") >= 1f)
                {
                    fsmSystem.SetState(StateType.Walk);
                }
            }
            else//空手
            {
                if (animSystem.IsAnimationComplete("UnholdSword") >= 1f)
                {
                    fsmSystem.SetState(StateType.Walk);
                }
            }
        }

        public override void OnEnter()
        {
            if (playerParam.isWeapon)
            {
                isGetting = false;
            }
            else
            {
                isGetting = true;
            }
            animSystem.BlendTree1DSetAnimSpeed("SwordMove", 1.25f);
            playerParam.isWeapon = !playerParam.isWeapon;
            time = 0f;
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
            time += Time.deltaTime;
            if (isGetting && time > 0.267f)
            {
                playerParam.DisPlayWeapon(true);
            }
            else if (!isGetting && time > 0.86f)
            {
                playerParam.DisPlayWeapon(false);
            }
        }
    }
}