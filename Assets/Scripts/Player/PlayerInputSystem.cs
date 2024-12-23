using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;
using System;

public class PlayerInputSystem : MonoBehaviour
{
    private PlayerAction inputController;

    #region 公有变量

    public Vector2 playerMovement
    {
        get => inputController.Player.PlayerMove.ReadValue<Vector2>();
    }

    public Vector2 playerMovementRaw
    {
        get => inputController.Player.PlayerMoveRaw.ReadValue<Vector2>();
    }

    public Vector2 cameraLook
    {
        get => inputController.Player.Look.ReadValue<Vector2>();
    }

    public bool playerJump
    {
        get => inputController.Player.Jump.triggered;
    }

    public bool playerDodge
    {
        get => inputController.Player.Dodge.triggered;
    }

    public bool playerSkill
    {
        get => inputController.Player.Skill.triggered;
    }

    public bool playerLockEnemy
    {
        get => inputController.Player.LockEnemy.triggered;
    }

    public bool playerSwitchSword
    {
        get => inputController.Player.SwitchSword.triggered;
    }

    public bool playerLAtk
    {
        get => inputController.Player.LAtk.triggered;
    }

    public bool playerRun;

    public bool playerCrouch;

    public bool playerDefend;

    #endregion 公有变量

    #region 私有变量

    [SerializeField]
    private Vector2 m_playerMovement;

    [SerializeField]
    private Vector2 m_playerMovementRaw;

    [SerializeField]
    private Vector2 m_cameraLook;

    [SerializeField]
    private bool m_playerJump;

    [SerializeField]
    private bool m_playerDodge;

    [SerializeField]
    private bool m_playerSkill;

    [SerializeField]
    private bool m_playerLockEnemy;

    [SerializeField]
    private bool m_playerSwitchSword;

    [SerializeField]
    private bool m_playerLAtk;

    #endregion 私有变量

    #region 脚本方法

    private void Awake()
    {
        if (inputController == null)
            inputController = new PlayerAction();

        inputController.Player.Run.performed += OnRunPerformed;

        inputController.Player.Crouch.performed += OnCrouchPerformed;

        inputController.Player.Defend.performed += OnDefendPerformed;
    }

    private void OnEnable()
    {
        inputController.Enable();
    }

    private void OnDisable()
    {
        inputController.Player.Run.performed -= OnRunPerformed;

        inputController.Player.Crouch.performed -= OnCrouchPerformed;

        inputController.Player.Defend.performed -= OnDefendPerformed;

        inputController.Disable();
    }

    private void LateUpdate()
    {
        m_playerMovement = inputController.Player.PlayerMove.ReadValue<Vector2>();
        m_playerMovementRaw = inputController.Player.PlayerMoveRaw.ReadValue<Vector2>();
        m_cameraLook = inputController.Player.Look.ReadValue<Vector2>();

        m_playerJump = inputController.Player.Jump.triggered;
        m_playerDodge = inputController.Player.Dodge.triggered;
        m_playerSkill = inputController.Player.Skill.triggered;
        m_playerLockEnemy = inputController.Player.LockEnemy.triggered;
        m_playerSwitchSword = inputController.Player.SwitchSword.triggered;
        m_playerLAtk = inputController.Player.LAtk.triggered;
    }

    #endregion 脚本方法

    #region 私有方法

    private void OnRunPerformed(InputAction.CallbackContext context)
    {
        playerRun = !playerRun;
    }

    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            playerCrouch = true;
        }
        else
        {
            playerCrouch = false;
        }
    }

    private void OnDefendPerformed(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            playerDefend = true;
        }
        else
        {
            playerDefend = false;
        }
    }

    #endregion 私有方法
}