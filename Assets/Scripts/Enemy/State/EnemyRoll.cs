﻿using ARPG.Animation;
using ARPG.FSM;

namespace ARPG.AI
{
    public class EnemyRoll : EnemyBaseState
    {
        private enum DIR
        {
            F, L, R, B
        }

        private DIR dir;

        public EnemyRoll(AnimSystem _animSystem, EnemyAI _enemyAI) : base(_animSystem, _enemyAI)
        {
        }

        public override void Condition()
        {
        }

        public override void OnEnter()
        {
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