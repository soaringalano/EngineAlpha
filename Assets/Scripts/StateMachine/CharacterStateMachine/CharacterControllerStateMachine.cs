using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CharacterControllerStateMachine : AbstractStateMachine<CharacterState>
{

    public const string KEY_STATUS_BOOL_TOUCHGROUND = "TouchGround";

    public const string KEY_STATUS_TRIGGER_ATTACK = "CommAttack";

    public const string KEY_STATUS_TRIGGER_ISHIT = "IsHit";

    public const string KEY_STATUS_TRIGGER_JUMP = "Jump";

    public const string KEY_STATUS_TRIGGER_INAIR = "InAir";

    public const string KEY_STATUS_TRIGGER_VICTORY = "Victory";

    public const string KEY_STATUS_FLOAT_MOVEX = "MoveX";

    public const string KEY_STATUS_FLOAT_MOVEY = "MoveY";

    public const string KEY_STATUS_FALL_HEIGHT = "FallHeight";

    private const float TO_RADIAN = Mathf.PI / 180;

    public Camera Camera { get; private set; }

    [field: SerializeField]
    public Rigidbody RB { get; private set; }

    [field: SerializeField]
    private Animator Animator { get; set; }

    [field: SerializeField]
    public float AccelerationValue { get; private set; }

    [field: SerializeField]
    public float MaxVelocity { get; private set; }

    [field: SerializeField]
    public float MaxForwardVelocity { get; private set; }

    [field: SerializeField]
    public float MaxBackwardVelocity { get; private set; }

    [field: SerializeField]
    public float MaxSideVelocity { get; private set; }

    [field: SerializeField]
    public float JumpIntensity { get; private set; } = 2000.0f;

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

    protected override void Awake()
    {
        base.Awake();
        m_enemies = FindEnemies();
    }

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new AttackState());
    }

    // Start is called before the first frame update
    void Start()
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

    private void Update()
    {
        m_currentState.OnUpdate();
        UpdateAnimatorBoolValue(KEY_STATUS_BOOL_TOUCHGROUND, m_floorTrigger.IsOnFloor);
        if(!m_floorTrigger.IsOnFloor && m_currentState is not JumpState)
        {
            ActivateInAirTrigger();
        }
        TryStateTransition();
        UpdateEnemies();
    }

    public void UpdateAnimatorBoolValue(string key, bool value)
    {
        Animator.SetBool(key, value);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();
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

    private void TryStateTransition()
    {
        if (!m_currentState.CanExit())
        {
            return;
        }

        //Je PEUX quitter le state actuel
        foreach (var state in m_possibleStates)
        {
            if (m_currentState == state)
            {
                continue;
            }

            if (state.CanEnter())
            {
                //Quitter le state actuel
                m_currentState.OnExit();
                m_currentState = state;
                //Rentrer dans le state state
                m_currentState.OnEnter();
                return;
            }
        }
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

    public void ActivateAttackAnimation()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_ATTACK);
    }

    public void EnableTouchGround()
    {
        Animator.SetBool(KEY_STATUS_BOOL_TOUCHGROUND, true);
    }

    public void DisableTouchGround()
    {
        Animator.SetBool(KEY_STATUS_BOOL_TOUCHGROUND, false);
    }

    public void UpdateAnimatorMovementValues(Vector2 movementValue)
    {
        //Aller chercher ma vitesse actuelle
        //Communiquer directment avec mon Animator
        Animator.SetFloat("MoveX", movementValue.x / MaxVelocity);
        Animator.SetFloat("MoveY", movementValue.y / MaxVelocity);
    }

    public void ActivateJumpTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_JUMP);
    }

    public void ActivateInAirTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_INAIR);
    }

    public void ActivateVictoryTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_VICTORY);
    }

    public void ActivateIsHitTrigger()
    {
        Animator.SetTrigger(KEY_STATUS_TRIGGER_ISHIT);
    }

}
