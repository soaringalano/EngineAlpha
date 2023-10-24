using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CharacterControllerStateMachine : AbstractStateMachine<CharacterState>, IDamageable
{
    public const string KEY_STATUS_BOOL_STUN = "Stun";

    public const string KEY_STATUS_BOOL_TOUCHGROUND = "TouchGround";

    public const string KEY_STATUS_TRIGGER_ATTACK = "CommAttack";

    public const string KEY_STATUS_TRIGGER_FALLONGROUND = "FallOnGround";

    public const string KEY_STATUS_TRIGGER_INAIR = "InAir";

    public const string KEY_STATUS_TRIGGER_ISHIT = "IsHit";

    public const string KEY_STATUS_TRIGGER_JUMP = "Jump";

    public const string KEY_STATUS_TRIGGER_VICTORY = "Victory";

    public const string KEY_STATUS_FLOAT_FALL_HEIGHT = "FallHeight";

    public const string KEY_STATUS_FLOAT_MOVEX = "MoveX";

    public const string KEY_STATUS_FLOAT_MOVEY = "MoveY";

    public const float TO_RADIAN = Mathf.PI / 180;

    public const float STUN_HEIGHT = 5.0f;

    public Camera Camera { get; private set; }

    [field: SerializeField]
    public Rigidbody RB { get; private set; }

    [field: SerializeField]
    private Animator Animator { get; set; }

    [field: SerializeField]
    public float AccelerationValue { get; private set; }

    private Vector2 CurrentRelativeVelocity { get; set; }

    public Vector2 CurrentDirectionalInputs { get; private set; }

    [field: SerializeField]
    public float DecelerationValue { get; private set; } = 0.3f;

    [field: SerializeField]
    public float MaxVelocity { get; private set; }

    [field: SerializeField]
    public float MaxForwardVelocity { get; private set; }

    [field: SerializeField]
    public float MaxBackwardVelocity { get; private set; }

    [field: SerializeField]
    public float MaxSideVelocity { get; private set; }

    public float InAirAccelerationValue { get; private set; } = 0.2f;

    [field: SerializeField]
    public float JumpIntensity { get; private set; } = 1000.0f;

    [field: SerializeField]
    public float CommAttackDamage { get; set; } = 10.0f;

    [SerializeField]
    private CharacterFloorTrigger m_floorTrigger;

    [field: SerializeField]
    private Vector2 m_secureFieldOfView;

    [field: SerializeField]
    private float m_life { get; set; }

    [field: SerializeField]
    private float m_enemyDamage { get; set; }

    private GameObject[] m_enemies;
    
    public bool OnHitStimuliReceived { get; set; } = false;
    
    public bool OnStunStimuliReceived { get; set; } = false;

    private Vector2 m_highestPosition = Vector2.zero;

    private Vector2 m_lowestPosition = Vector2.positiveInfinity;

    public List<AudioSource> m_audioSources;

    protected override void Awake()
    {
        base.Awake();
        m_enemies = FindEnemies();
    }

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState(m_audioSources[0]));
        m_possibleStates.Add(new JumpState(m_audioSources[1]));
        m_possibleStates.Add(new InAirState());
        m_possibleStates.Add(new OnGroundState(m_audioSources[2]));
        m_possibleStates.Add(new AttackState(m_audioSources[3]));
        m_possibleStates.Add(new HitState(m_audioSources[4]));
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Camera = Camera.main;
        //m_rigitBody = GetComponent<Rigidbody>();

        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }

    protected override void Update()
    {
        /*if(!m_floorTrigger.IsOnFloor && m_currentState is not JumpState)
        {
            ActivateInAirTrigger();
        }
        m_currentState.OnUpdate();
        TryStateTransition();*/
        UpdateAnimatorKeyValues();
        base.Update();
    }

    public void UpdateAnimatorKeyValues()
    {
        UpdateAnimatorBoolValue(KEY_STATUS_BOOL_TOUCHGROUND, m_floorTrigger.IsOnFloor);
        Animator.SetFloat("MoveX", CurrentRelativeVelocity.x / GetCurrentMaxSpeed());
        Animator.SetFloat("MoveY", CurrentRelativeVelocity.y / GetCurrentMaxSpeed());
        UpdateEnemies();

    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        //Debug.Log("Current state:" + m_currentState.GetType());
        SetDirectionalInputs();
        m_currentState.OnFixedUpdate();
        Set2dRelativeVelocity();
    }

    // Only called by InAirState to ensure only when in air it's called
    public void InAirFixedUpdateFallHeight()
    {
        if(m_highestPosition.y < transform.position.y)
        {
            m_highestPosition = transform.position;
        }    
        if(m_lowestPosition.y >  transform.position.y)
        {
            m_lowestPosition = transform.position;
        }
        float f = m_highestPosition.y - m_lowestPosition.y;
        if (f > 0)
        {
            SetFloatFallHeight(f);
        }
        else
        {
            SetFloatFallHeight(0.0f);
        }
    }

    public void InAirResetFallHeight()
    {
        m_highestPosition = Vector2.zero;
        m_lowestPosition = Vector2.positiveInfinity;
        SetFloatFallHeight(0);
    }

    public bool IsFallingFromHigh()
    {
        return Animator.GetFloat(KEY_STATUS_FLOAT_FALL_HEIGHT) > STUN_HEIGHT;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.name == "Arms")
        {
            ActivateIsHitTrigger();
        }
        if (collision.collider.name == "WinCondition")
        {
            if (collision.collider == null || collision.collider.IsDestroyed())
            {
                ActivateVictoryTrigger();
            }
        }
    }

    public void Jump()
    {
        RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
    }

    public void ReceiveDamage(EDamageType damageType)
    {

        if (damageType == EDamageType.Normal)
        {
            OnHitStimuliReceived = true;
        }
        if (damageType == EDamageType.Stunning)
        {
            OnStunStimuliReceived = true;
        }
    }

    public void FixedUpdateQuickDeceleration()
    {
        var oppositeDirectionForceToApply = -RB.velocity * DecelerationValue * Time.fixedDeltaTime;
        RB.AddForce(oppositeDirectionForceToApply, ForceMode.Acceleration);
    }

    public bool IsInContactWithFloor()
    {
        return m_floorTrigger.IsOnFloor;
    }
    GameObject[] FindEnemies()
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == 10)
            {
                goList.Add(goArray[i]);
            }
        }
        return goList.Count == 0 ? null : goList.ToArray();
    }

    public List<Collider> GetAttackableEnemies()
    {
        /**
         * ray cast detect surrounding objects, each object should be collide trigger and implements trigger method, for example, wall's trigger
         * is do nothing but enemy trigger will cause loss of life to both
         */
        if(m_enemies == null || m_enemies.Length == 0)
        {
            return null;
        }
        List<Collider> ret = new List<Collider>();
        foreach (GameObject enemy in m_enemies)
        {
            if (enemy == null || enemy.IsDestroyed())
            {
                //Debug.Log("This enemy object has already been destroyed");
                continue;
            }
            Vector3 v3 = enemy.transform.position - RB.transform.position;
            //Debug.Log("Enemy distance:" + v3.x + "," + v3.y + "," + v3.z);
            float angle = Vector3.Angle(v3 * TO_RADIAN, RB.transform.forward);
            if (angle <= m_secureFieldOfView.y && v3.magnitude <= m_secureFieldOfView.x)
            {
                ret.Add(enemy.GetComponent<Collider>());
            }
        }
        return ret.Count == 0 ? null : ret;
    }

    public void UpdateEnemies()
    {
        m_enemies = FindEnemies();
    }

    public void UpdateAnimatorBoolValue(string key, bool value)
    {
        Animator.SetBool(key, value);
    }

    private void Set2dRelativeVelocity()
    {
        Vector3 relativeVelocity = RB.transform.InverseTransformDirection(RB.velocity);

        CurrentRelativeVelocity = new Vector2(relativeVelocity.x, relativeVelocity.z);
    }

    public float GetCurrentMaxSpeed()
    {

        if (Mathf.Approximately(CurrentDirectionalInputs.magnitude, 0))
        {
            return MaxForwardVelocity;
        }

        var normalizedInputs = CurrentDirectionalInputs.normalized;

        var currentMaxVelocity = Mathf.Pow(normalizedInputs.x, 2) * MaxSideVelocity;

        if (normalizedInputs.y > 0)
        {
            currentMaxVelocity += Mathf.Pow(normalizedInputs.y, 2) * MaxForwardVelocity;
        }
        else
        {
            currentMaxVelocity += Mathf.Pow(normalizedInputs.y, 2) * MaxBackwardVelocity;
        }

        return currentMaxVelocity;
    }

    public void SetDirectionalInputs()
    {
        CurrentDirectionalInputs = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            CurrentDirectionalInputs += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            CurrentDirectionalInputs += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            CurrentDirectionalInputs += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            CurrentDirectionalInputs += Vector2.right;
        }
    }

    public void EnableTouchGround()
    {
        Animator.SetBool(KEY_STATUS_BOOL_TOUCHGROUND, true);
    }

    public void DisableTouchGround()
    {
        Animator.SetBool(KEY_STATUS_BOOL_TOUCHGROUND, false);
    }

    public void EnableStun()
    {
        Animator.SetBool(KEY_STATUS_BOOL_STUN, true);
    }

    public void DisableStun()
    {
        Animator.SetBool(KEY_STATUS_BOOL_STUN, false);
    }

    public void ActivateAttackTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_ATTACK);
    }

    public void ActivateFallOnGroundTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_FALLONGROUND);
    }

    public void ActivateInAirTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_INAIR);
    }

    public void ActivateIsHitTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_ISHIT);
    }

    public void ActivateJumpTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_JUMP);
    }

    public void ActivateVictoryTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_VICTORY);
    }

    public void SetFloatFallHeight(float fallHeight)
    {
        Animator.SetFloat(KEY_STATUS_FLOAT_FALL_HEIGHT, fallHeight);
    }

}
