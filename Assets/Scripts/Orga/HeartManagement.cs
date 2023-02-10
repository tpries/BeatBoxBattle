using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManagement : MonoBehaviour
{

    [SerializeField] CharacterBase player;
    [SerializeField] private int amount;
    [SerializeField] GameObject heartPrefab;
    
    private Vector2 firstHeartPosition;
    private Vector2 offSet;

    [SerializeField] private Vector3 positionOffset;
    private GameObject[] hearts;

    private int currentHeart;

    // Start is called before the first frame update
    void Awake()
    {
        firstHeartPosition = positionOffset + transform.parent.transform.position;

        amount = player.GetLifes();

        currentHeart = amount;

        offSet = new Vector2(25f, 0f);

        hearts = new GameObject[amount];
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = Instantiate(heartPrefab, firstHeartPosition + offSet * i, heartPrefab.transform.rotation);
            hearts[i].transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        // avoiding index issues
        if(currentHeart == player.GetLifes() || player.GetLifes()<0 || player.GetLifes() > amount) { return; }

        if(currentHeart > player.GetLifes())
        {
            // Player lost a heart
            hearts[currentHeart - 1].GetComponent<Animator>().SetBool("Dead",true);
            currentHeart--;
        }
        else
        {
            // Player gained a heart
            hearts[currentHeart].GetComponent<Animator>().SetBool("Dead",false);
            currentHeart++;
        }
    }

    // On property change lifes of player
}
