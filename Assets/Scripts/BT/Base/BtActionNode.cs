using ARPG.BB;
using System;
using UnityEngine;

namespace ARPG.BT
{
    public class BtActionNode : BtBehaviour
    {
        protected EnemyController enemyController;
        protected Func<EStatus> action;

        public BtActionNode(EnemyController enemyController, Func<EStatus> action)
        {
            this.enemyController = enemyController;
            this.action = action;
        }

        protected override EStatus OnUpdate()
        {
            return action();
        }
    }
}