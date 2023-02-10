using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadHighScores: MonoBehaviour
{
    void Awake()
    {
        GlobalManager.ReadHighScores();
    }
}