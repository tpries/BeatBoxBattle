using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    public Rigidbody2D GetRigidBody() { return rb; }

    protected Animator animator;
    public Animator GetAnimator() { return animator; }

    [SerializeField] protected float moveSpeed = 0;
    protected float jumpForce = 0;

    protected float moveHorizontal;
    protected float moveVertical;

    [SerializeField]protected bool facingLeft = true;

    [SerializeField] protected int lifes = 0;
    protected int startLifes;
    public void SetLifes(int setLifes) { lifes = setLifes; }
    public int GetLifes() { return lifes; }

    protected bool isJumping = false;

    protected bool doubleJump = true;
    protected float doubleJumpOffset = 0.25f;
    protected bool noMoreJumps = false;

    protected float lastJump;
    protected float jumpCoolDown = 0.5f;

    protected bool ducking = false;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if(jumpCoolDown < Time.time - lastJump) { jumpBlock = false; }

        if(transform.position.y < -20) 
        {
            rb.velocity = Vector2.zero;
            lifes = 0;
            TakeDamage(100); 
        }
    }

    protected void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    void FixedUpdate()
    {
        if (lifes <= 0) { return; }

        //animator.SetBool("Jumping", isJumping);
        if (moveVertical > 0.1f && !noMoreJumps)
        {
            if (isJumping && !doubleJump && doubleJumpOffset < Time.time - lastJump) 
            {
                doubleJump = true;
                noMoreJumps = true;

                lastJump = Time.time;

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if (!isJumping)
            {
                isJumping = true;
                doubleJump = false;

                lastJump = Time.time;

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        if (isJumping && moveVertical < -0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce);
        }
        else if (!isJumping && moveVertical < -0.1f && !ducking)
        {
            // duck
            Vector3 scale = transform.localScale;
            scale.y /= 2f;


            Vector3 position = transform.position;
            position.y -= 0.5f;

            transform.position = position;
            transform.localScale = scale;
            
            ducking = true;
        }
        
        if (ducking && moveVertical > -0.1f)
        {
            ducking = false;
            Vector3 scale = transform.localScale;
            scale.y *= 2f;

            Vector3 position = transform.position;
            position.y += 0.5f;

            transform.position = position;
            transform.localScale = scale;
        }

        if (moveHorizontal == 0) 
        { 
            //animator.SetBool("Moving", false);
            return;
        }


        // abkürzen 
        float inAirFactor;

        if (isJumping) { inAirFactor = 0.5f; }
        else { inAirFactor = 1f; }

        if (moveHorizontal > 0.1f)
        {

            if (facingLeft)
            {
                Flip();
                facingLeft = false;
            }

            rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
            //rb.AddForce(new Vector2(moveHorizontal * moveSpeed * inAirFactor, 0f), ForceMode2D.Impulse);
        }
        else if (moveHorizontal < -0.1f)
        {
            //animator.SetBool("Moving", true);


            if (!facingLeft)
            {
                Flip();
                facingLeft = true;
            }
            rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);

            //rb.AddForce(new Vector2(moveHorizontal * moveSpeed * inAirFactor, 0f), ForceMode2D.Impulse);
        }
    }

    protected void TakeDamage(int damage)
    {   

        if ((damage > 0 && lifes > 0) || (damage < 0 && lifes < startLifes)) 
        { 
            //animator.SetTrigger("Damage");
            lifes = lifes - damage;
        }

        if (lifes <= 0)
        {
            GlobalManager.CompletedLevel();
            //freeze = 0f;
            //animator.SetTrigger("Death");
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        //gameObject.layer = LayerMask.NameToLayer("Dead");

        if(gameObject.tag == "Player")
        {
            GlobalManager.LoadScene("GameOverScene");
        }

        yield return new WaitForSeconds(0f);//animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return lifes > 0;
    }
}
