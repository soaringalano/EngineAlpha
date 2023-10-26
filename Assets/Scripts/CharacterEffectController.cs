using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectController : MonoBehaviour
{

    [SerializeField]
    public List<SpecialFX> m_specialFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

public enum EFXState
{
    EAttack, EWalk, EHit, EFall, EJump
}

public struct SpecialFX
{
    EFXState state;
    List<AudioClip> audioClips;
    List<ParticleSystem> particleSystems;

    void PlayAudio(int index, Vector3 position)
    {
        AudioClip clip = audioClips[index];
        AudioSource.PlayClipAtPoint(clip, position);
    }

    void PlayAudio(Vector3 position)
    {
        int index = Random.Range(0, audioClips.Count);
        PlayAudio(index, position);
    }

    void PlayParticle(int index)
    {
        ParticleSystem particle = particleSystems[index];
        particle.Play();
    }

    void PlayParticle()
    {
        int index = Random.Range(0, particleSystems.Count);
        PlayParticle(index);
    }
}
