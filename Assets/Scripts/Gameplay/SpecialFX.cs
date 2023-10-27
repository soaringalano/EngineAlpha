using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFX
{

    public EFXState state;
    public List<AudioClip> audioClips;
    public List<ParticleSystem> particleSystems;

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