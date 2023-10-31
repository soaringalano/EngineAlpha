using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CharacterEffectController : MonoBehaviour
{

    [SerializeField]
    public List<SpecialFX> SpecialFXs = new List<SpecialFX>();

    [SerializeField]
    private Dictionary<EFXState, SpecialFX> SpecialFXsMap = new Dictionary<EFXState, SpecialFX>();

    private void Awake()
    {
        foreach (SpecialFX sfx in SpecialFXs)
        {
            SpecialFXsMap.Add(sfx.state, sfx);
        }
    }

    public void PlaySoundFX(EFXState state, Vector3 position, float volume)
    {
        SpecialFX sfx = SpecialFXsMap[state];
        if(sfx.audioClips.Count > 0 )
        {
            AudioSource.PlayClipAtPoint(sfx.audioClips[0], position, volume);
        }
    }

    public void PlayParticleFX(EFXState state, Vector3 position)
    {
        SpecialFX pfx = SpecialFXsMap[state];
        if(pfx.particleSystems.Count > 0)
        {
            pfx.particleSystems[0].Play();

        }
    }

}

public enum EFXState
{
    EOpening, EAttack, EWalk, EHit, EFall, EJump, EVictory, EGameEnd, ECutScene
}

[System.Serializable]
public struct SpecialFX
{
    public EFXState state;
    public List<AudioClip> audioClips;
    public List<ParticleSystem> particleSystems;
}