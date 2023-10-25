using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectController : MonoBehaviour
{

    [SerializeField]
    public Dictionary<ESoundFXState, List<AudioSource>> m_soundFxMapping;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum ESoundFXState
    {
        EAttack, EWalk, EHit, EFall, EJump
    }
}
