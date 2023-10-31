using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is to deactivate or activate according to the state
public class HitboxController : MonoBehaviour
{

    private CharacterControllerStateMachine m_characterController;

    //Left hand : stun, right hand : hit
    //a stun punch will activate special effect
    [field:SerializeField]
    public List<GameObject> m_hitboxes {  get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        DeactivateHitbox();
        m_characterController = GetComponent<CharacterControllerStateMachine>();
    }

    public void ActivateHitbox()
    {
        if(m_hitboxes != null)
        {
            foreach(GameObject hitbox in m_hitboxes)
            {
                if(hitbox != null)
                {
                    hitbox.SetActive(true);
                }
            }
        }
    }

    public void DeactivateHitbox()
    {
        if (m_hitboxes != null)
        {
            foreach (GameObject hitbox in m_hitboxes)
            {
                if (hitbox != null)
                {
                    hitbox.SetActive(false);
                }
            }
        }
    }

}
