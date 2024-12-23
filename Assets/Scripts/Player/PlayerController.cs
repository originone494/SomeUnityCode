using ARPG.Animation;
using ARPG.FSM;
using ARPG.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateType current;
    public AnimSetting[] animSetting;

    private Animator am;
    private CharacterController cc;

    private AnimSystem animSystem;
    private FSMSystem fsmSystem;
    private PlayerInputSystem inputSystem;
    private PlayerParam playerParam;

    private void Awake()
    {
        am = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        inputSystem = GetComponent<PlayerInputSystem>();
        animSystem = new AnimSystem(am, animSetting);
        playerParam = GetComponent<PlayerParam>();
        fsmSystem = new FSMSystem(cc, animSystem, inputSystem, playerParam);
    }

    private void Update()
    {
        fsmSystem.OnUpdate();

        getPararm();
    }

    private void getPararm()
    {
        if (fsmSystem != null)
        {
            current = fsmSystem.currentStateType;
        }
    }

    private void LateUpdate()
    {
        fsmSystem.OnFixedUpdate();
    }

    private void FixedUpdate()
    {
        fsmSystem.OnFixedUpdate();
    }

    private void OnDisable()
    {
        animSystem.graph.Destroy();
    }
}