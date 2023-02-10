using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : ButtonToScene
{
    private bool x = false;

    void Start()
    {
        x = true;
    }

    void OnMouseDown()
    {
        Application.Quit();
    }
}