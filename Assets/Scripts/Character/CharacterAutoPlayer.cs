using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

// this class will affect on any object that accept keyboard input, you are able to setup a series of keys to make object play automatically
public class CharacterAutoPlayer : MonoBehaviour
{

    private bool m_executed = false;

    [SerializeField]
    public float AutoplayDuration;

    private List<float> m_autoplayTimer = new List<float>();

    private float m_delayTimer = 0.0f;

    [SerializeField]
    public List<int> m_keys = new List<int>();

    [SerializeField]
    public List<float> m_pressDuration = new List<float>();

    [SerializeField]
    public List<float> m_pressDelays = new List<float>();

    private List<float> m_keyTimers = new List<float>();

    private List<int> m_flags = new List<int>();


    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    static extern void keybd_event
    (
        byte bVk,           // virtual key value
        byte bScan,         // 0
        int dwFlags,        // 0 for keydown, 1 for hold key, 2 for release key
        int dwExtraInfo     // 0
    );

    private void PressKey(int key)
    {
        Debug.Log("automatic press " +  key);
        keybd_event((byte)key, 0, 0, 0);
    }

    private void HoldKey(int key)
    {
        Debug.Log("automatic press & hold " + key);
        keybd_event((byte)key, 0, 1, 0);
    }

    private void ReleaseKey(int key)
    {
        Debug.Log("automatic release " + key);
        keybd_event((byte)key, 0, 2, 0);
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        m_delayTimer = 0.0f;
        for(int i=0; i<m_keys.Count; i++)
        {
            m_flags.Add(0);
            m_autoplayTimer.Add(0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_executed) return;

        if(0 >= AutoplayDuration)
        {
            for (int i = 0; i < m_keys.Count; i++)
            {
                if (m_flags[i] < 2)
                {
                    ReleaseKey((byte)m_keys[i]);
                    m_flags[i] = 2;
                }
            }
            m_executed = true;
            //gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        for(int i=0;i<m_keys.Count;i++)
        {
            int key = m_keys[i];
            int flag = m_flags[i];
            float delay = m_pressDelays[i];
            float duration = m_pressDuration[i];
            float playTimer = m_autoplayTimer[i];
            if(delay <= m_delayTimer)
            {
                if(flag == 0)
                {
                    PressKey(key);
                    m_flags[i] = 1;
                }
                else if (flag == 1)
                {
                    float time = m_autoplayTimer[i];
                    if(time >= duration)
                    {
                        ReleaseKey((byte)key);
                        m_flags[i] = 2;
                    }
                    else
                    {
                        HoldKey((byte)key);
                    }
                }
                m_autoplayTimer[i] = playTimer + Time.fixedDeltaTime;
            }
        }
        m_delayTimer += Time.fixedDeltaTime;
        AutoplayDuration -= Time.fixedDeltaTime;
    }
}
