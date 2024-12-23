using ARPG.AI;
using ARPG.Animation;

namespace ARPG.FSM
{
    public abstract class EnemyBaseState : BaseState
    {
        protected EnemyAI enemyAI;

        public EnemyBaseState(AnimSystem _animSystem, EnemyAI _enemyAI) : base(_animSystem)
        {
            enemyAI = _enemyAI;
        }
    }
}