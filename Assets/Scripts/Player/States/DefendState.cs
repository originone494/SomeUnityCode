using ARPG.Animation;
using UnityEngine;

namespace ARPG.FSM
{
    public class DefendState : PlayerBaseState
    {
        public DefendState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem, _inputSystem, _playerParam, _fsmSystem)
        {
        }

        public override void Condition()
        {
            if (!inputSystem.playerDefend)
            {
                fsmSystem.SetState(StateType.Walk);
            }
            else if (Input.GetKeyDown(KeyCode.K))//受到伤害
            {
                fsmSystem.SetState(StateType.Damage);
            }
        }

        public override void OnEnter()
        {
            playerParam.ApplyRotation = false;
            playerParam.RootMotion = false;
        }

        public override void OnExit()
        {
            playerParam.ApplyRotation = true;
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