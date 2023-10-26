using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{

    [field: SerializeField]
    private float m_life { get; set; }

    [field: SerializeField]
    private float m_angularVelocity { get; set; }

    [field: SerializeField]
    private float m_commAttackDamage { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float angle = m_angularVelocity * Time.fixedDeltaTime;
        transform.Rotate(0, angle, 0);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.gameObject.name == "MainCharacter")
        {
            if(m_life <= 0)
            {
                Destroy(gameObject);
                return;
            }
            m_life -= m_commAttackDamage;
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
