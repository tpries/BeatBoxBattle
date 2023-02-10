using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancerStartScreen : MonoBehaviour
{

    private bool letsgo = false;
    private Animator animator;
    private float startTime;
    private float animLength = 4.85f;
    [SerializeField] Camera camera;

    void Awake()
    {
        GlobalManager.FindAudioManager();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GlobalManager.PlayStartSceneBackgroundMusic();

        if (GlobalManager.playing && Time.time - startTime >= animLength)
        {
            GlobalManager.LoadScene("SampleScene");
        }
    }

    public void LetsGo() 
    { 
        letsgo = true;
        transform.Rotate(0f, 180f, 0f);
        animator.SetTrigger("LetsGo");
        startTime = Time.time;
        camera.GetComponent<Animator>().SetTrigger("LetsGo");
    }
}
