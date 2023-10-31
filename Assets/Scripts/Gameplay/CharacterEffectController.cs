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
        DontDestroyOnLoad(this);
        foreach (SpecialFX sfx in SpecialFXs)
        {
            SpecialFXsMap.Add(sfx.state, sfx);
        }
    }

    void PlaySoundFX(EFXState state, Vector3 position, float volume)
    {
        SpecialFX sfx = SpecialFXsMap[state];
        AudioSource.PlayClipAtPoint(sfx.audioClips[0], position, volume);
    }

    void PlayParticleFX(EFXState state, Vector3 position)
    {
        SpecialFX pfx = SpecialFXsMap[state];
        pfx.particleSystems[0].Play();
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