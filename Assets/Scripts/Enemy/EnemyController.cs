using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    private float m_timer;

    [SerializeField]
    private float m_waitTime;

    [field: SerializeField]
    private float m_life { get; set; }

    [field: SerializeField]
    private float m_angularVelocity { get; set; }

    [field: SerializeField]
    private float m_commAttackDamage { get; set; }

    [field: SerializeField]
    private AudioClip m_audioClip { get; set; }

    [field: SerializeField]
    private ParticleSystem m_particleSystem { get; set; }

    private bool m_audiaoPlayed = false;

    private bool m_particlePlayed = false;

    //[SerializeField]
    private CharacterControllerStateMachine m_characterController;


    void Awake()
    {
        m_timer = m_waitTime;

        m_characterController = GameObject.Find("MainCharacter").GetComponent<CharacterControllerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(m_timer <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }*/
    }

    void FixedUpdate()
    {
        float angle = m_angularVelocity * Time.fixedDeltaTime;
        transform.Rotate(0, angle, 0);

        if (m_life <= 0 && !m_audiaoPlayed && !m_particlePlayed)
        {
            m_particleSystem.Play(true);
            m_particlePlayed = true;
            AudioSource.PlayClipAtPoint(m_audioClip, transform.position);
            m_audiaoPlayed = true;
        }
        if (m_particlePlayed && m_audiaoPlayed)
        {
            m_timer -= Time.fixedDeltaTime;
        }
        if(m_timer <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            m_characterController.SetEnemyDefeated(true);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.gameObject.name == "HitboxLeft" ||
            collision.collider.gameObject.name == "HitboxRight" ||
            collision.collider.gameObject.name == "MainCharacter")
        {
            CharacterControllerStateMachine characterControllerSM =
                collision.collider.GetComponentInParent<CharacterControllerStateMachine>();
            characterControllerSM.ReceiveDamage(EDamageType.Normal, m_commAttackDamage);
            m_angularVelocity *= -1;
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        /*if (collision.collider.gameObject.name == "MainCharacter")
        {
            if (m_life <= 0)
            {
                //Destroy(gameObject);
                return;
            }
            m_life -= m_commAttackDamage;
        }*/
    }

    public void OnCollisionExit(Collision collision)
    {
        
    }

    public void ReceiveDamage(EDamageType damageType, float damage)
    {
        switch(damageType)
        {
            case EDamageType.Count:
            {
                    m_life -= m_commAttackDamage;
                    break;
            }
            default: break;
        }
    }
}
