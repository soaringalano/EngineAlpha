public abstract class CharacterState : IState
{
    protected CharacterControllerStateMachine m_stateMachine;

    protected EFXState m_efxState;

    protected SpecialFX m_specialFX;

    public void OnStart(CharacterControllerStateMachine stateMachine)
    {
        m_stateMachine = stateMachine;
    }
    
    public virtual void OnStart()
    {
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual bool CanEnter(IState currentState)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CanExit()
    {
        throw new System.NotImplementedException();
    }

    public EFXState GetEFXState()
    {
        return m_efxState;
    }

    public void SetEFXState( EFXState state )
    { 
        m_efxState = state;
    }

}
