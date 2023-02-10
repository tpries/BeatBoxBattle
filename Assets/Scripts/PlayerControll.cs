using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : CharacterBase
{
    // Start is called before the first frame update
    void Start()
    {
        base.startLifes = base.lifes;
        base.facingLeft = false;
        base.jumpForce = 15;
        base.rb = gameObject.GetComponent<Rigidbody2D>();
        //base.animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -100)
        {
            // might be a problem with the HeartManagement
            base.TakeDamage(100);
            //GlobalManager.LoadScene("StartScene");
        }

        base.Update();

        base.moveHorizontal = Input.GetAxisRaw("Horizontal");

        base.moveVertical = Input.GetAxisRaw("Vertical");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            transform.parent = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            transform.parent = other.transform;
            base.isJumping = false;
            base.noMoreJumps = false;
        }

        if (other.gameObject.tag == "Beat")
        {
            Destroy(other.gameObject);

            /*
            if (transform.position.y > other.gameObject.transform.position.y + 0.8f)
            {
                GlobalManager.SurvivedBeat();
            }
            else
            {
                base.TakeDamage(1);
            }
            */
            base.TakeDamage(1);
        }

        if (other.gameObject.tag == "Heart")
        {
            base.TakeDamage(-1);
            Destroy(other.gameObject);
        }
    }
}
