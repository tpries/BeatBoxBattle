using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSceneToMain : MonoBehaviour
{
    private float duration = 1.8f;
    private float startTime;

    void Awake()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime >= duration)
        {
            GlobalManager.LoadScene("StartScreen");
        }
    }

}