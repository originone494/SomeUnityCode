using ARPG.Animation;
using ARPG.BB;
using ARPG.FSM;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.AI
{
    public enum EnemyStateType
    {
        None,
        Idle,
        Defend,
        Damage,
        Attack,
        Skill,
        Roll,
        Move
    }

    public class EnemyAI
    {
        private Animator am;
        public CharacterController cc;
        private AnimSystem animSystem;
        public EnemyController enemyController;
        public BlackBoard blackBoard;

        public BaseState currentState;
        public EnemyStateType currentStateType;
        private Dictionary<EnemyStateType, BaseState> allSaveState;
        //public Strategy strategy;

        public EnemyAI(CharacterController _cc, AnimSystem _animSystem, EnemyController _enemyController, BlackBoard _blackBoard)
        {
            cc = _cc;
            animSystem = _animSystem;
            enemyController = _enemyController;
            blackBoard = _blackBoard;

            allSaveState = new Dictionary<EnemyStateType, BaseState>();

            AddState(EnemyStateType.Idle, new EnemyIdle(_animSystem, this));

            SetState(EnemyStateType.Idle);
            currentStateType = EnemyStateType.Idle;
            //strategy = Strategy.Observe;
        }

        #region 状态机方法

        public void AddState(EnemyStateType stateType, BaseState state)
        {
            if (allSaveState.ContainsKey(stateType))
            {
                return;
            }
            allSaveState.Add(stateType, state);
        }

        public void SwitchAnim(EnemyStateType stateType, float enterTime = -1)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    animSystem.TransitionTo("Idle", enterTime);
                    break;

                case EnemyStateType.Defend:
                    animSystem.TransitionTo("Defend", enterTime);
                    break;

                case EnemyStateType.Damage:
                    animSystem.TransitionTo("Damage", enterTime);
                    break;

                case EnemyStateType.Move:
                    animSystem.TransitionTo("Move", enterTime);
                    break;

                case EnemyStateType.Roll:

                    break;

                case EnemyStateType.Skill:

                    break;

                case EnemyStateType.Attack:

                    break;
            }
        }

        public void SetState(EnemyStateType stateType, float enterTime = -1f)
        {
            if (currentState == allSaveState[stateType])
            {
                return;
            }
            currentStateType = stateType;
            currentState?.OnExit();
            currentState = allSaveState[stateType];
            currentState?.OnEnter();
        }

        #endregion 状态机方法

        #region 脚本方法

        public void OnUpdate()
        {
            currentState?.Condition();
            currentState?.OnUpdate();
        }

        public void OnFixedUpdate()
        {
            currentState?.OnFixedUpdate();
        }

        public void OnLaterUpdate()

        {
            currentState?.OnLaterUpdate();
        }

        #endregion 脚本方法

        #region 私有方法

        public void BeAttacked()
        {
            // Debug.Log("BeAttacked --enemy");
            SetState(EnemyStateType.Damage);
        }

        #endregion 私有方法
    }
}