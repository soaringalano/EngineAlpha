using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is to slow down the action of character while attacking.
public class HitboxPunchTrigger : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void OnTriggerEnter(Collider other)
    {

        Time.timeScale = 0.5f;
    }

    public void OnTriggerExit(Collider other)
    {

        Time.timeScale = 1.0f;
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Time.timeScale = 0.5f;
    }

    private void OnCollisionExit(Collision collision)
    {
        Time.timeScale = 1.0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

}
