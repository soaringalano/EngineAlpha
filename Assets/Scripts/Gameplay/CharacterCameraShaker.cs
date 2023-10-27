using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;

using UnityEngine;

public class CharacterCameraShaker : MonoBehaviour
{
    public CameraShake cameraShake;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(cameraShake.Shake(0.5f, 0.1f));
        }
    }
}