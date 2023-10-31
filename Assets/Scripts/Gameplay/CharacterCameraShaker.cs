using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;

using UnityEngine;

//Deprecated
public class CharacterCameraShaker : MonoBehaviour
{
    [SerializeField]
    private CameraShake cameraShake;

    private void Awake()
    {
    }

    public void ShakeCamera()
    {
        StartCoroutine(cameraShake.Shake(0.5f, 0.1f));
    }
}