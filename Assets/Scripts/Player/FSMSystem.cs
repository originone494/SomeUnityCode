using ARPG.Animation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ARPG.FSM
{
    public enum StateType
    {
        None,
        Idle,
        Crouch,
        Walk,
        Run,
        Jump,
        Fall,
        Dodge,
        Defend,
        SwitchWeapon,
        Stop,
        Skill,
        Damage,
        Attack
    }

    public class FSMSystem
    {
        public BaseState currentState;
        public StateType currentStateType;
        private Dictionary<StateType, BaseState> allSaveState;

        private AnimSystem animSystem;
        private PlayerParam playerParam;
        private PlayerInputSystem inputSystem;

        public FSMSystem(CharacterController _cc, AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam)
        {
            animSystem = _animSystem;
            playerParam = _playerParam;
            inputSystem = _inputSystem;

            allSaveState = new Dictionary<StateType, BaseState>();

            AddState(StateType.Idle, new IdleState(_animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Walk, new MoveState(_animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Jump, new JumpState(_cc, _animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Fall, new FallState(_cc, _animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Dodge, new DodgeState(_animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.SwitchWeapon, new SwitchWeaponState(_animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Stop, new StopState(_animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Skill, new SkillState(_animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Damage, new DamageState(_animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Attack, new AttackState(_animSystem, _inputSystem, _playerParam, this));
            AddState(StateType.Defend, new DefendState(_animSystem, _inputSystem, _playerParam, this));

            SetState(StateType.Idle);
            currentStateType = StateType.Idle;
        }

        public void AddState(StateType stateType, BaseState state)
        {
            if (allSaveState.ContainsKey(stateType))
            {
                return;
            }
            allSaveState.Add(stateType, state);
        }

        public void SetState(StateType stateType, float enterTime = -1f)
        {
            if (currentState == allSaveState[stateType])
            {
                return;
            }
            currentStateType = stateType;
            currentState?.OnExit();

            switch (stateType)
            {
                case StateType.Idle:
                    animSystem.TransitionTo("Idle", enterTime);
                    break;

                case StateType.Walk:
                    if (playerParam.isWeapon)
                    {
                        animSystem.TransitionTo("SwordMove", enterTime);
                    }
                    else
                    {
                        animSystem.TransitionTo("Move", enterTime);
                    }
                    break;

                case StateType.Dodge:
                    if (inputSystem.playerMovement != Vector2.zero)
                    {
                        animSystem.TransitionTo("Dodge_F", enterTime);
                    }
                    else
                    {
                        animSystem.TransitionTo("Dodge_B", enterTime);
                    }
                    break;

                case StateType.Jump:
                    animSystem.TransitionTo("Jump", enterTime);
                    break;

                case StateType.Fall:
                    animSystem.TransitionTo("Fall", enterTime);
                    break;

                case StateType.Stop:
                    animSystem.TransitionTo("RunStop", enterTime);
                    break;

                case StateType.SwitchWeapon:
                    if (playerParam.isWeapon)
                    {
                        animSystem.TransitionTo("UnholdSword", enterTime);
                    }
                    else
                    {
                        animSystem.TransitionTo("HoldSword", enterTime);
                    }
                    break;

                case StateType.Skill:
                    animSystem.TransitionTo("Skill", enterTime);
                    break;

                case StateType.Damage:
                    animSystem.TransitionTo("Damage", enterTime);
                    break;

                case StateType.Attack:
                    animSystem.TransitionTo("LAtk_1", enterTime);
                    break;

                case StateType.Defend:
                    animSystem.TransitionTo("Defend", enterTime);
                    break;
            }

            currentState = allSaveState[stateType];
            currentState?.OnEnter();
        }

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
    }
}