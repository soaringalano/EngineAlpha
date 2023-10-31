using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingStairController : MonoBehaviour
{

    [field: SerializeField]
    private float m_breakForce { get; set; }

    [field: SerializeField]
    private float m_floatingSpeed { get; set; }

    [field: SerializeField]
    private List<Rigidbody> m_docks { get; set; } = new List<Rigidbody>();

    private Rigidbody m_currentDock;

    private int m_currentDockIndex = -1;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_docks.Count < 2)
        {
            Debug.Log("number of docks for floating stair can not be less than 2");
            return;
        }
        m_currentDockIndex++;
        m_currentDock = m_docks[m_currentDockIndex];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_currentDock != null)
        {
            Transform tran = gameObject.GetComponentInParent<Transform>();
            Vector3 dockPos = m_currentDock.transform.position;
            Vector3 direction = (dockPos - tran.position).normalized;
            tran.Translate(direction * m_floatingSpeed * Time.fixedDeltaTime, Space.World);
            //Vector3 movement = (dockPos - gameObject.transform.position).normalized;
            //movement *= m_floatingSpeed * Time.fixedDeltaTime;
            //gameObject.transform.position += movement;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.name == "MainCharacter")
        {
            if(gameObject.GetComponent<FixedJoint>() != null)
            {
                Destroy(gameObject.GetComponent<FixedJoint>());
            }
        }
    }

    public void OnCollisionStay(Collision collision)
    {

        if (collision.collider.gameObject.name == "MainCharacter")
        {
            return;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.name == "MainCharacter")
        {
            Debug.Log("MainCharacter collided with stairway");
            FixedJoint joint = gameObject.GetComponent<FixedJoint>();
            if (joint == null)
            {
                joint = gameObject.AddComponent<FixedJoint>();
            }
            joint.connectedBody = collision.collider.GetComponent<Rigidbody>();
            joint.breakForce = m_breakForce;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "MainCharacter")
        {
            return;
        }
        if (m_docks.Count == 2)
        {
            m_currentDockIndex = (m_currentDockIndex + 1) % m_docks.Count;
        }
        else if(m_docks.Count > 2)
        {
            int index = Random.Range(0, m_docks.Count);
            if (index == m_currentDockIndex)
            {
                m_currentDockIndex = (index + 1) % m_docks.Count;
            }
            else
            {
                m_currentDockIndex = index;
            }
        }
        m_currentDock = m_docks[m_currentDockIndex];
    }

}
