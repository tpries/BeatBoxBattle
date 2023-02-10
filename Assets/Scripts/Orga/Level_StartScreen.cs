using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_StartScreen : ButtonToScene
{

    [SerializeField] private int levelNumber;
    [SerializeField] private Transform plotPoint;

    void Start()
    {
        Debug.Log("Start" + Time.time.ToString());
        SetBestTime();
    }

    void SetBestTime()
    {
        GlobalManager.PlotDigits(GlobalManager.GetBestScore(levelNumber), plotPoint, false);
    }

    void OnMouseDown()
    {
        if (GlobalManager.playing) { return; }

        GlobalManager.StartLevel(levelNumber);
        GameObject.Find("Dancer").GetComponent<DancerStartScreen>().LetsGo();
    }
}