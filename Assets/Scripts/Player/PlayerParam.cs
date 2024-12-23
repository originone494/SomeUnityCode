using Assets.Scripts;
using System.Text.RegularExpressions;
using Tiny;
using UnityEngine;

public class PlayerParam : MonoBehaviour
{
    public enum ATKDIR
    {
        FL, FR, F, B, None
    }

    public bool OnGround; // 是否接触地面
    public Vector3 velocity;// 当前速度
    public bool RootMotion;
    public bool ApplyRotation;
    public Vector3 RotateForward;

    public bool isWeapon;//是否佩戴武器
    public bool isAttacking;//是否正在进行攻击
    public bool isEndAttacking;//是否结束当前的攻击，可以进入下一次攻击
    public int hitCount;//击中次数
    public ATKDIR atkDirection;
    public float AttackDamage;//造成的伤害

    public GameObject HandWeapon;
    public GameObject BackWeapon;
    private Trail SwordTrailScript;

    public AudioSource audioSource;
    public AudioClip moveAudioClip;
    public AudioClip attackAudioClip;

    public float moveMaxWeight;

    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float jumpMaxSpeed;

    public float enterIdleTime;//站着不动进入待机状态的时间

    public float gravity;
    public float groundDetectionRadius;

    [HideInInspector] public Transform PlayerModel;
    [HideInInspector] public Transform CameraTransform;

    #region 跳跃水平速度相关变量

    private static readonly int CACHE_SIZE = 3;
    private Vector3[] velCache = new Vector3[CACHE_SIZE];
    private int currentChacheIndex = 0;
    public Vector3 averageVel = Vector3.zero;

    #endregion 跳跃水平速度相关变量

    #region 所需组件

    [HideInInspector] public CharacterController cc;
    private Animator am;
    private PlayerInputSystem inputSystem;
    private PlayerController playerController;

    #endregion 所需组件

    #region 脚本方法

    private void Awake()
    {
        CameraTransform = Camera.main.transform;

        cc = GetComponent<CharacterController>();
        am = GetComponent<Animator>();
        PlayerModel = transform;
        inputSystem = GetComponent<PlayerInputSystem>();
        playerController = GetComponent<PlayerController>();

        isWeapon = false;
        SwordTrailScript = HandWeapon.GetComponent<Trail>();
        SwordTrailScript.enabled = false;
        HandWeapon.SetActive(false);
        BackWeapon.SetActive(true);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.rotation = new Quaternion(0f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        Rotate();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void OnDrawGizmos()
    {
        if (cc == null) return;

        // 起点计算
        Vector3 start = transform.position + (Vector3.up * groundDetectionRadius);

        // 半径和长度
        float sphereRadius = cc.radius;
        float rayLength = groundDetectionRadius - sphereRadius;// + 2 * cc.skinWidth;

        // 方向
        Vector3 direction = Vector3.down;

        // 绘制球体起点
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(start, sphereRadius);

        // 绘制射线路径
        Gizmos.color = Color.yellow;
        Vector3 end = start + direction * rayLength;
        Gizmos.DrawLine(start, end);

        // 绘制球体终点
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(end, sphereRadius);
    }

    private void OnAnimatorMove()
    {
        if (RootMotion && !GameManager.Instance.blackBoard.GetBool(GameManager.Instance.isInteractString))
        {
            Vector3 Movement = am.deltaPosition;
            Movement.y = velocity.y * Time.fixedDeltaTime;
            cc.Move(Movement);
            averageVel = AverageVel(am.velocity);
        }
        //Debug.Log(am.velocity);
    }

    #endregion 脚本方法

    #region 公有方法

    public void DisPlayWeapon(bool display)
    {
        if (display)
        {
            HandWeapon.SetActive(true);
            BackWeapon.SetActive(false);
        }
        else
        {
            HandWeapon.SetActive(false);
            BackWeapon.SetActive(true);
        }
    }

    #endregion 公有方法

    #region 私有方法

    private Vector3 AverageVel(Vector3 newVel)
    {
        velCache[currentChacheIndex] = newVel;
        currentChacheIndex++;
        currentChacheIndex %= CACHE_SIZE;
        Vector3 average = Vector3.zero;
        foreach (Vector3 vel in velCache)
        {
            average += vel;
        }
        return average / CACHE_SIZE;
    }

    private void Rotate()
    {
        if (ApplyRotation)
        {
            float rotateSpeed = 500;

            Vector3 camForwardProjection = new Vector3(CameraTransform.forward.x, 0, CameraTransform.forward.z).normalized;
            RotateForward = camForwardProjection * inputSystem.playerMovement.y + CameraTransform.right * inputSystem.playerMovement.x;
            RotateForward = PlayerModel.InverseTransformVector(RotateForward);

            float rad = Mathf.Atan2(RotateForward.x, RotateForward.z);
            PlayerModel.Rotate(0, rad * rotateSpeed * Time.deltaTime, 0f);
        }
    }

    private void CheckGround()
    {
        if (Physics.SphereCast(PlayerModel.position + (Vector3.up * groundDetectionRadius),
           cc.radius,
           Vector3.down, out RaycastHit hit,
           groundDetectionRadius - cc.radius + 1.5f * cc.skinWidth))
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }
    }

    //0为否，1为是
    private void SwitchAttacking(string info)
    {
        Match matchINT = Regex.Match(info, @"\d+");
        int i = matchINT.Success ? int.Parse(matchINT.Value) : 0;

        string atkDir = Regex.Replace(info, @"[^a-zA-Z]", "");

        //Debug.Log(atkDir);

        if (i == 1)
        {
            isEndAttacking = true;

            hitCount = 0;

            SwordTrailScript.enabled = true;

            isAttacking = true;

            audioSource.clip = attackAudioClip;
            audioSource.volume = 1f;

            audioSource.Play();

            switch (atkDir)
            {
                case "FL":
                    atkDirection = ATKDIR.FL;
                    break;

                case "FR":
                    atkDirection = ATKDIR.FR;
                    break;

                case "F":
                    atkDirection = ATKDIR.F;
                    break;

                case "B":
                    atkDirection = ATKDIR.B;
                    break;

                default:
                    Debug.Log("攻击方向错误");
                    atkDirection = ATKDIR.None;
                    break;
            }
        }
        else if (i == 0)
        {
            SwordTrailScript.enabled = false;
            atkDirection = ATKDIR.None;
            isAttacking = false;
            AttackDamage = 0f;
        }
    }

    private void MoveSound()
    {
        if (moveMaxWeight > 0.6f)
        {
            audioSource.clip = moveAudioClip;
            audioSource.volume = 0.5f;

            audioSource.Play();
        }
    }

    private void AttackSound()
    {
        audioSource.clip = attackAudioClip;
        audioSource.volume = 1f;

        audioSource.Play();
    }

    private void InstantiateEffect()
    {
    }

    #endregion 私有方法
}