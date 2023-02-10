using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToScene : MonoBehaviour
{
    [SerializeField] private string goToScene;

    private SpriteRenderer renderer;

    Color offHoverColor;
    Color onHoverColor;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        offHoverColor = new Color(1f, 1f, 1f, 0.7f);
        onHoverColor = new Color(1f, 1f, 1f, 1f);
    }

    void OnMouseOver()
    {
        if (GlobalManager.playing) { return; }
        renderer.color = onHoverColor;
    }

    void OnMouseExit()
    {
        if (GlobalManager.playing) { return; }
        renderer.color = offHoverColor;
    }

    void OnMouseDown()
    {
        if (GlobalManager.playing) { return; }
        GlobalManager.LoadScene(goToScene);
    }
}