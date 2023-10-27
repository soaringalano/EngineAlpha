using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{

    [field:SerializeField]
    public List<GameObject> m_hitboxes {  get; set; }

    private void Awake()
    {
        DeactivateHitbox();
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
