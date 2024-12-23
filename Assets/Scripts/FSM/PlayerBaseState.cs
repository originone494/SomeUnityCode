using ARPG.Animation;
using ARPG.FSM;

public abstract class PlayerBaseState : BaseState
{
    protected PlayerInputSystem inputSystem;
    protected PlayerParam playerParam;
    protected FSMSystem fsmSystem;

    public PlayerBaseState(AnimSystem _animSystem, PlayerInputSystem _inputSystem, PlayerParam _playerParam, FSMSystem _fsmSystem) : base(_animSystem)
    {
        inputSystem = _inputSystem;
        playerParam = _playerParam;
        fsmSystem = _fsmSystem;
    }
}