using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    private PlayerInputSystem _playerInput;
    [SerializeField] private Transform LookAttarGet;
    private Transform playerCamera;

    [Range(0.1f, 1.0f), SerializeField, Header("鼠标灵敏度")] public float mouseInputSpeed;

    [SerializeField, Header("相机对于玩家")] private float normalRadius;
    [SerializeField] private float currentRadius;
    private Vector2 ClmpCameraRang = new Vector2(-85f, 70f);

    [SerializeField, Header("锁敌")] private bool isLockOn = false;
    private Transform currentEnemy;
    private float escapeDistance;

    [SerializeField, Header("滑轮")] private float zoomSpeed = 1f;
    [SerializeField] private Vector2 minMaxZoom = new Vector2(4f, 7f);
    private float zoom = 1f;

    [SerializeField, Header("相机碰撞")] public LayerMask collisionLayer;

    private Vector3 rotationSmoothVelocity;
    private float yaw;
    private float pitch;

    private void Start()
    {
        normalRadius = 5f;

        playerCamera = Camera.main.transform;
        _playerInput = LookAttarGet.transform.root.GetComponent<PlayerInputSystem>();
    }

    private void ControllerCamera()
    {
        //相机缩放
        if (zoom != 0)
        {
            normalRadius -= zoom * normalRadius;
            normalRadius = Mathf.Clamp(normalRadius, minMaxZoom.x, minMaxZoom.y);
        }

        //相机旋转
        if (!isLockOn)
        {
            Quaternion targetRotation = Quaternion.Euler(new Vector3(pitch, yaw));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.4f);

            if (pitch < -8f)
            {
                currentRadius = Mathf.Lerp(currentRadius, normalRadius / 2f, 0.1f);
            }
            else
            {
                currentRadius = Mathf.Lerp(currentRadius, normalRadius, 0.1f);
            }
        }

        //相机碰撞检测
        Vector3 fanlePos = Vector3.zero;
        if (Physics.Linecast(LookAttarGet.position + Vector3.up, playerCamera.position + (-playerCamera.forward.normalized), out var hit, collisionLayer))
        {
            float currentDistance = Vector3.Distance(hit.point, LookAttarGet.position);
            //Debug.Log("发生碰撞 " + currentDistance);

            if (currentDistance <= minMaxZoom.y)
            {
                currentRadius = currentDistance * 0.95f;
            }
        }

        //相机跟踪
        fanlePos = LookAttarGet.position - transform.forward * currentRadius;
        transform.position = Vector3.Lerp(transform.position, fanlePos, 0.8f);
    }

    private void GetCameraControllerInput()
    {
        if (isLockOn) return;

        yaw += _playerInput.cameraLook.x * mouseInputSpeed;
        pitch -= _playerInput.cameraLook.y * mouseInputSpeed;
        pitch = Mathf.Clamp(pitch, ClmpCameraRang.x, ClmpCameraRang.y);

        zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }

    private void CameraLockOnTarget()
    {
        if (!isLockOn) return;

        Vector3 EnemyDir = ((currentEnemy.position + currentEnemy.transform.up * 0.3f) - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(EnemyDir.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
    }

    private void LockEnemy()
    {
        if (_playerInput.playerLockEnemy)
        {
            Debug.Log("锁定敌人");
            if (currentEnemy == null)
            {
                Vector3 tempPosition = LookAttarGet.transform.position;
                Vector3 center = tempPosition + new Vector3(0, 1.0f, 0) + transform.forward * 5.0f;

                Collider[] col = Physics.OverlapBox(center, new Vector3(10f, 5f, 10f), transform.rotation, LayerMask.GetMask("Enemy"));
                if (col.Length != 0)
                {
                    currentEnemy = col[0].transform;
                    isLockOn = true;
                }
            }
            else
            {
                UnLock();
            }
        }

        //距离太远，自动解除锁定
        if (currentEnemy != null)
        {
            if (Vector3.Distance(LookAttarGet.position, currentEnemy.position) > escapeDistance)
            {
                UnLock();
            }
        }
    }

    private void UnLock()
    {
        currentEnemy = null;
        isLockOn = false;
    }

    private void OnDrawGizmos()
    {
        //Vector3 tempPosition = LookAttarGet.transform.position;
        //Vector3 center = tempPosition + new Vector3(0, 1.0f, 0) + transform.forward * 5.0f;
        //Gizmos.DrawCube(center, new Vector3(10f, 5f, 10f));
    }

    private void Update()
    {
        GetCameraControllerInput();//计算
        LockEnemy();//选择锁定的敌人
    }

    private void LateUpdate()
    {
        ControllerCamera();//移动摄像头
        CameraLockOnTarget();//锁定敌人
    }
}