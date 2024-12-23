using ARPG.BB;
using ARPG.BT;
using UnityEngine;
using System;
using static PlayerParam;
using Assets.Scripts;
using ARPG.UI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyAnim
    {
        None,
        Idle,
        Defend,
        Hit,
        Attack,
        SkillA,
        SkillB,
        SkillC,
        Dodge,
        Move
    }

    #region 所需组件

    //public AnimSetting[] animSetting;
    //public AnimSystem animSystem;
    private BehaviorTreeBuilder builder;

    private BlackBoard blackBoard;

    private Animator am;
    private CharacterController cc;

    #endregion 所需组件

    #region 属性变量键值

    public EnemyAnim currentAnim;

    public float distance;
    public float attackDistance;
    //public float nearDistance;
    //public float midDistance;
    //public float farDistance;

    public Vector3 velocity;
    public bool OnGround;
    public float gravity;
    public bool RootMotion;
    public float turnSpeed;
    public bool isDefend;
    public int healthPoint;
    public int DefendPower;

    private AudioSource audioSource;
    public AudioClip defendSound;

    public GameObject HitEffect;
    public GameObject ReboundEffect;
    public Transform[] HitPos;

    //动画名称
    private const string IdleString = "Idle";

    private const string DefendString = "Defend";
    private const string DodgeBString = "Dodge_B";
    private const string DodgeFString = "Dodge_F";
    private const string DodgeLString = "Dodge_L";
    private const string DodgeRString = "Dodge_R";
    private const string MoveString = "Move";
    private const string Hit_Normal_FB_String = "Hit_Normal_FB";
    private const string Hit_Normal_FR_String = "Hit_Normal_FR";
    private const string Hit_Normal_FL_String = "Hit_Normal_FL";
    private const string Hit_Normal_F_String = "Hit_Normal_F";
    private const string Hit_Defend_String = "Hit_Defend";
    private const string Hit_Rebound_String = "Hit_Rebound";
    private const string Hit_Large_String = "Hit_Large";
    private const string Attack1String = "Attack1";
    private const string Attack4String = "Attack4";
    private const string SkillAString = "SkillA";
    private const string SkillB1String = "SkillB_1";
    private const string SkillB2String = "SkillB_2";
    private const string SkillCString = "SkillC";

    //动画机哈希值
    private int SpeedFloatHash;

    private int TurnFloatHash;
    private int AttackTriggerHash;
    private int SkillATriggerHash;
    private int SkillBTriggerHash;
    private int SkillCTriggerHash;
    private int DefendBoolHash;
    private int DodgeTriggerHash;
    private int FHitTriggerHash;
    private int FLHitTriggerHash;
    private int FRHitTriggerHash;
    private int BHitTriggerHash;
    private int DefendHitTriggerHash;
    private int ReboundHitTriggerHash;
    private int LargeHitTriggerHash;

    //黑板，用于记录cd
    private string SkillAKey = "SkillA";

    private string SkillBKey = "SkillB";
    private string SkillCKey = "SkillC";
    private string CanSkillKey = "Skill";
    private string CanAttackKey = "Attack";
    private string CanDodgeKey = "Dodge";
    private string ObserveConvertKey = "ObserveDir";

    public int SkillACD;
    public int SkillBCD;
    public int SkillCCD;
    public int SkillCD;
    public int AttackCD;
    public int DodgeCD;

    #endregion 属性变量键值

    #region AI视野相关变量

    public Transform detectionCenter;
    public float detectRadius;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsBos;

    private Collider[] colliderTarget = new Collider[1];

    public Transform currentTarget;

    #endregion AI视野相关变量

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        //组件
        am = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //animSystem = new AnimSystem(am, animSetting);
        builder = new BehaviorTreeBuilder();
        blackBoard = GetComponent<BlackBoard>();

        //当前状态
        currentAnim = EnemyAnim.Idle;

        //动画控制器哈希
        SpeedFloatHash = Animator.StringToHash("Speed");
        TurnFloatHash = Animator.StringToHash("Turn");
        AttackTriggerHash = Animator.StringToHash("Attack");
        SkillATriggerHash = Animator.StringToHash("SkillA");
        SkillBTriggerHash = Animator.StringToHash("SkillB");
        SkillCTriggerHash = Animator.StringToHash("SkillC");
        DefendBoolHash = Animator.StringToHash("Defend");
        DodgeTriggerHash = Animator.StringToHash("Dodge");
        FHitTriggerHash = Animator.StringToHash("Hit_F");
        FLHitTriggerHash = Animator.StringToHash("Hit_FL");
        FRHitTriggerHash = Animator.StringToHash("Hit_FR");
        BHitTriggerHash = Animator.StringToHash("Hit_B");
        DefendHitTriggerHash = Animator.StringToHash("Hit_Defend");
        ReboundHitTriggerHash = Animator.StringToHash("Hit_Rebound");
        LargeHitTriggerHash = Animator.StringToHash("Hit_Large");

        //行为树
        BtBehaviour IsFindPlayerCondition = new BtActionNode(this, IsFindPlayer);
        BtBehaviour EnterAtkDisCondition = new BtActionNode(this, EnterAtkDis);
        BtBehaviour EnterDashDisCondition = new BtActionNode(this, EnterDashDis);
        BtBehaviour EnterLAtkDisCondition = new BtActionNode(this, EnterLtkDis);
        BtBehaviour EnterObeservehDisCondition = new BtActionNode(this, EnterObeservehDis);
        BtBehaviour CanAttackCondition = new BtActionNode(this, CanAttack);
        BtBehaviour CanDodgeCondition = new BtActionNode(this, CanDodge);
        BtBehaviour CanSkillCondition = new BtActionNode(this, CanSkill);
        BtBehaviour CanSkillACondition = new BtActionNode(this, CanSkillA);
        BtBehaviour CanSkillBCondition = new BtActionNode(this, CanSkillB);
        BtBehaviour BeAttackCondition = new BtActionNode(this, IsBeAttacked);

        BtBehaviour NearPlayerAciton = new BtActionNode(this, NearPlayer);
        BtBehaviour AttackPlayerAction = new BtActionNode(this, AttackPlayer);
        BtBehaviour RollBackwardsAction = new BtActionNode(this, RollBackwards);
        BtBehaviour ObservePlayerAction = new BtActionNode(this, ObservePlayer);
        BtBehaviour BackOffAction = new BtActionNode(this, BackOff);
        BtBehaviour DashSkillAction = new BtActionNode(this, UseDashSkill);
        BtBehaviour CloseSkillAction = new BtActionNode(this, UseCloseSkill);
        BtBehaviour IdleAction = new BtActionNode(this, Idle);
        BtBehaviour BeAttackAcion = new BtActionNode(this, BeAttacked);

        builder
            .Seletctor()//1
                .Sequence()
                    .AddBehavior(BeAttackCondition)
                    .AddBehavior(BeAttackAcion)
                .Back()
                .Sequence()//11
                    .AddBehavior(IsFindPlayerCondition)//111
                        .Seletctor()//112
                            .Sequence()//1121

                                .Sequence()
                                    .Inverter()
                                    .AddBehavior(EnterAtkDisCondition)
                                    .Back()
                                .Back()

                                .Seletctor()
                                    .Sequence()//1121121
                                        .AddBehavior(EnterDashDisCondition)
                                        .AddBehavior(CanSkillCondition)
                                        .AddBehavior(CanSkillACondition)
                                        .AddBehavior(DashSkillAction)
                                    .Back()
                                    .Seletctor()
                                        .Seletctor()
                                            .Sequence()
                                                .AddBehavior(CanAttackCondition)
                                                .AddBehavior(NearPlayerAciton)
                                            .Back()
                                            .Sequence()
                                                .AddBehavior(EnterObeservehDisCondition)
                                                .AddBehavior(ObservePlayerAction)
                                            .Back()
                                        .Back()
                                    .Back()
                                .Back()
                            .Back()
                            .Sequence()//1122
                                .AddBehavior(EnterAtkDisCondition)
                                    .Seletctor()
                                        .Sequence()//是否能够攻击
                                            .AddBehavior(CanAttackCondition)
                                            .AddBehavior(AttackPlayerAction)
                                            .Back()
                                        .Seletctor()
                                            .Sequence()//能否使用近战技能
                                                .AddBehavior(CanSkillCondition)
                                                .AddBehavior(CanSkillBCondition)
                                                .AddBehavior(CloseSkillAction)
                                            .Back()
                                            .Seletctor()
                                                .Sequence()//翻滚
                                                    .AddBehavior(CanDodgeCondition)
                                                    .AddBehavior(RollBackwardsAction)
                                                .Back()
                                                .AddBehavior(BackOffAction)
                                            .Back()
                                        .Back()
                                    .Back()
                            .Back()//1122
                        .Back()//112
                .Back()//11
                .Sequence()//12

                    .Sequence()
                        .Inverter()
                        .AddBehavior(IsFindPlayerCondition)
                        .Back()
                     .Back()

                    .AddBehavior(IdleAction)
                .Back()//12
            .Back()//1
        .End();

        //黑板
        blackBoard.SetValue(SkillAKey, true);
        blackBoard.SetValue(SkillBKey, true);
        blackBoard.SetValue(SkillCKey, true);
        blackBoard.SetValue(CanSkillKey, true);
        blackBoard.SetValue(CanAttackKey, true);
        blackBoard.SetValue(CanDodgeKey, true);

        blackBoard.SetValue(ObserveConvertKey, true);
    }

    #region 脚本方法

    private void Update()
    {
        transform.rotation = new Quaternion(0f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        GetDistance();
        CheckGround();
    }

    private void FixedUpdate()
    {
        builder.TreeTick();
    }

    #endregion 脚本方法

    #region 私有方法

    private GameObject SpawnHit(GameObject effect)
    {
        GameObject spawnedHit = Instantiate(effect);
        spawnedHit.transform.LookAt(Camera.main.transform);
        spawnedHit.transform.position = HitPos[UnityEngine.Random.Range(0, HitPos.Length)].position;
        return spawnedHit;
    }

    private float GetDistance()
    {
        if (currentTarget != null)
        {
            distance = Vector3.Distance(transform.position, currentTarget.position);
        }
        else
        {
            distance = 1e6f;
        }
        return distance;
    }

    private void CheckGround()
    {
        if (Physics.SphereCast(transform.position + (Vector3.up * 0.5f),
           cc.radius,
           Vector3.down, out RaycastHit hit,
           0.5f - cc.radius + 1.5f * cc.skinWidth))
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }
    }

    private void OnAnimatorMove()
    {
        if (RootMotion)
        {
            if (distance > 0.6f)
            {
                Vector3 Movement = am.deltaPosition;
                Movement.y = velocity.y * Time.fixedDeltaTime;
                Movement *= Time.timeScale;
                cc.Move(Movement);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (detectionCenter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(detectionCenter.position, detectRadius); // 绘制检测范围
        }

        if (currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, currentTarget.position + Vector3.up * 0.5f); // 绘制目标指向线
        }
    }

    private void OnDisable()
    {
        builder.OnDisable();
    }

    private void SetAnimBlend2DFloat(float speed, float turn)
    {
        am.SetFloat(SpeedFloatHash, speed);
        am.SetFloat(TurnFloatHash, turn);
    }

    private bool GetAnimationIsComplete(string name)
    {
        AnimatorStateInfo stateInfo = am.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(name) && stateInfo.normalizedTime >= 0.99f)
        {
            return true;
        }
        return false;
    }

    private float GetAnimationDuringTime(string name)
    {
        AnimatorStateInfo stateInfo = am.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(name))
        {
            return stateInfo.normalizedTime;
        }
        Debug.Log("名字不对 ");
        return 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.OpenPanel(UIConst.EnemyStatePanel);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ClosePanel(UIConst.EnemyStatePanel);
        }
    }

    #endregion 私有方法

    #region 公有方法

    public void BeAttacked(float damage, PlayerParam.ATKDIR aTKDIR)
    {
        AnimSetState(EnemyAnim.Hit);

        DefendPower--;
        if (DefendPower == 0)
        {
            isDefend = false;
            SpawnHit(ReboundEffect);
            am.SetTrigger(ReboundHitTriggerHash);
            return;
        }

        if (isDefend)
        {
            SpawnHit(HitEffect);
            am.SetTrigger(DefendHitTriggerHash);
            return;
        }

        switch (aTKDIR)
        {
            case ATKDIR.FL:
                am.SetTrigger(FLHitTriggerHash);
                break;

            case ATKDIR.FR:
                am.SetTrigger(FRHitTriggerHash);
                break;

            case ATKDIR.F:
                am.SetTrigger(FHitTriggerHash);
                break;

            case ATKDIR.B:
                am.SetTrigger(BHitTriggerHash);
                break;

            default:
                Debug.Log("攻击方向错误");
                break;
        }
    }

    public void AnimSetState(EnemyAnim nextState)
    {
        if (currentAnim == nextState) return;

        currentAnim = nextState;

        switch (nextState)
        {
            case EnemyAnim.Idle:
                SetAnimBlend2DFloat(0f, 0f);
                break;

            case EnemyAnim.Defend:
                SetAnimBlend2DFloat(0f, 0f);
                break;

            case EnemyAnim.Dodge:
                SetAnimBlend2DFloat(0f, 0f);
                am.SetTrigger(DodgeTriggerHash);
                blackBoard.SetValue(CanDodgeKey, false, DodgeCD, true);
                break;

            case EnemyAnim.Move:
                break;

            case EnemyAnim.Hit:
                SetAnimBlend2DFloat(0f, 0f);
                break;

            case EnemyAnim.Attack:
                SetAnimBlend2DFloat(0f, 0f);
                am.SetTrigger(AttackTriggerHash);
                blackBoard.SetValue(CanAttackKey, false, AttackCD, true);
                break;

            case EnemyAnim.SkillA:
                SetAnimBlend2DFloat(0f, 0f);
                am.SetTrigger(SkillATriggerHash);
                blackBoard.SetValue(SkillAKey, false, SkillACD, true);
                blackBoard.SetValue(CanSkillKey, false, SkillCD, true);
                break;

            case EnemyAnim.SkillB:
                SetAnimBlend2DFloat(0f, 0f);
                am.SetTrigger(SkillBTriggerHash);
                blackBoard.SetValue(SkillBKey, false, SkillBCD, true);
                blackBoard.SetValue(CanSkillKey, false, SkillCD, true);
                break;

            case EnemyAnim.SkillC:
                SetAnimBlend2DFloat(0f, 0f);
                am.SetTrigger(SkillCTriggerHash);
                blackBoard.SetValue(SkillCKey, false, SkillCCD, true);
                blackBoard.SetValue(CanSkillKey, false, SkillCD, true);
                break;
        }
    }

    #endregion 公有方法

    #region 行为树方法

    public EStatus IsFindPlayer()
    {
        if (currentTarget != null && GetDistance() <= detectRadius * 0.8f)
        {
            return EStatus.Success;
        }

        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectRadius, colliderTarget, whatIsEnemy);

        if (targetCount > 0)
        {
            if (!Physics.Raycast(
                transform.position + transform.up * 0.5f,
                (colliderTarget[0].transform.position - transform.position).normalized,
                out var hit,
                detectRadius,
                whatIsBos
                ))
            {
                if (Vector3.Dot(((colliderTarget[0].transform.position + Vector3.up * 0.5f) - (transform.position + Vector3.up * 0.5f)).normalized, transform.forward) > 0.25f)
                {
                    currentTarget = colliderTarget[0].transform;
                    return EStatus.Success;
                }
            }
        }

        currentTarget = null;
        return EStatus.Failure;
    }

    public EStatus NearPlayer()
    {
        Debug.Log("接近玩家");
        AnimSetState(EnemyAnim.Move);
        SetAnimBlend2DFloat(1f, 0f);

        Quaternion q = Quaternion.LookRotation(currentTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed);

        return EStatus.Success;
    }

    public EStatus AttackPlayer()
    {
        //朝向玩家
        Quaternion q = Quaternion.LookRotation(currentTarget.position - transform.position);
        transform.rotation = q;

        AnimSetState(EnemyAnim.Attack);

        return EStatus.Success;
    }

    public EStatus RollBackwards()
    {
        Debug.Log("Dodge");
        AnimSetState(EnemyAnim.Dodge);

        Debug.Log(GetAnimationDuringTime(DodgeBString));

        if (currentAnim == EnemyAnim.Dodge && GetAnimationIsComplete(DodgeBString))
        {
            Debug.Log("Edn");
            return EStatus.Success;
        }

        return EStatus.Success;
    }

    public EStatus ObservePlayer()
    {
        Debug.Log("观察玩家");

        AnimSetState(EnemyAnim.Move);

        if (blackBoard.GetBool(ObserveConvertKey))
        {
            SetAnimBlend2DFloat(0f, -1f);
            blackBoard.SetValue(ObserveConvertKey, blackBoard.GetBool(ObserveConvertKey), 10, false);
        }
        else
        {
            blackBoard.SetValue(ObserveConvertKey, blackBoard.GetBool(ObserveConvertKey), 10, true);
            SetAnimBlend2DFloat(0f, 1f);
        }

        Quaternion q = Quaternion.LookRotation(currentTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed * 2f);

        return EStatus.Success;
    }

    public EStatus BackOff()
    {
        Debug.Log("BackOff");

        AnimSetState(EnemyAnim.Move);
        SetAnimBlend2DFloat(-1f, 0f);

        Quaternion q = Quaternion.LookRotation(currentTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed * 2f);

        if (GetDistance() >= attackDistance * 1.2f)
        {
            return EStatus.Success;
        }

        return EStatus.Running;
    }

    public EStatus EnterAtkDis()
    {
        if (GetDistance() <= attackDistance)
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus EnterDashDis()
    {
        if (GetDistance() <= attackDistance * 1.2f)
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus EnterObeservehDis()
    {
        if (GetDistance() >= attackDistance * 1.5f)
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus EnterLtkDis()
    {
        if (GetDistance() <= attackDistance * 0.5f)
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus CanAttack()
    {
        if (blackBoard.GetBool(CanAttackKey))
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus CanDodge()
    {
        if (blackBoard.GetBool(CanDodgeKey))
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus CanSkill()
    {
        if (blackBoard.GetBool(CanSkillKey))
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus CanSkillA()
    {
        if (blackBoard.GetBool(SkillAKey))
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus CanSkillB()
    {
        if (blackBoard.GetBool(SkillBKey))
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus UseDashSkill()
    {
        //朝向玩家
        Quaternion q = Quaternion.LookRotation(currentTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.9f);

        Debug.Log("冲刺技能");
        AnimSetState(EnemyAnim.SkillA);

        if (currentAnim == EnemyAnim.SkillA && GetAnimationIsComplete(SkillAString))
        {
            return EStatus.Success;
        }

        return EStatus.Success;
    }

    public EStatus UseCloseSkill()
    {
        //朝向玩家
        Quaternion q = Quaternion.LookRotation(currentTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed * 1.5f);

        Debug.Log("近战技能");
        AnimSetState(EnemyAnim.SkillB);

        if (currentAnim == EnemyAnim.SkillB && GetAnimationIsComplete(SkillB2String))
        {
            return EStatus.Success;
        }

        return EStatus.Success;
    }

    public EStatus Idle()
    {
        Debug.Log("Idle");

        AnimSetState(EnemyAnim.Idle);

        return EStatus.Success;
    }

    public EStatus IsBeAttacked()
    {
        if (currentAnim == EnemyAnim.Hit)
        {
            return EStatus.Success;
        }
        return EStatus.Failure;
    }

    public EStatus BeAttacked()
    {
        AnimatorStateInfo info = am.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName(Hit_Defend_String) && !info.IsName(Hit_Large_String)
            && !info.IsName(Hit_Normal_FB_String) && !info.IsName(Hit_Normal_FL_String)
                && !info.IsName(Hit_Normal_FR_String) && !info.IsName(Hit_Normal_F_String)
                && !info.IsName(Hit_Rebound_String))
        {
            return EStatus.Success;
        }

        return EStatus.Running;
    }

    #endregion 行为树方法

    #region 动画方法

    private void SwitchAttacking(int EffectNumber)
    {
    }

    private void DefendSound()
    {
        audioSource.clip = defendSound;

        audioSource.Play();
    }

    private void ReboundEvent()
    {
        GameManager.Instance.blackBoard.SetValue(GameManager.Instance.TimeSlowSpeedString, 0.1f, 1f, 1f);
    }

    #endregion 动画方法
}