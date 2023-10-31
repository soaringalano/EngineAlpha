using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is to shake camera while attacking or being hit.
public class CameraShakerOnHit : MonoBehaviour
{

    [SerializeField]
    public CinemachineVirtualCamera m_virtualCam;

    [SerializeField]
    public float m_hitAmplitudeGain = 2.0f;

    [SerializeField]
    public float m_hitFrequenceyGain = 2.0f;

    [SerializeField]
    public float m_shakeDuration = 0.2f;

    private CinemachineBasicMultiChannelPerlin m_perlin;

    private bool m_isShaking = false;

    private float m_shakeTimer = 0.0f;

    void Awake()
    {
        DontDestroyOnLoad(this);
        m_perlin = m_virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCameraOnHit()
    {
        m_isShaking = true;
        m_perlin.m_AmplitudeGain = m_hitAmplitudeGain;
        m_perlin.m_FrequencyGain = m_hitFrequenceyGain;
        m_shakeTimer = 0.0f;
    }

    public void StopShaking()
    {
        m_isShaking = false;
        m_perlin.m_FrequencyGain = 0.0f;
        m_perlin.m_AmplitudeGain = 0.0f;
        m_shakeTimer = 0.0f;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (m_isShaking)
        {
            m_shakeTimer += Time.fixedDeltaTime;
            if(m_shakeTimer >= m_shakeDuration)
            {
                StopShaking();
            }
        }
    }

}
