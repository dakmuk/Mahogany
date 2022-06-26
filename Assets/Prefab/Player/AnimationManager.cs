using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public Animator animator;
    public bool onGround;
    public bool onWall;
    public bool isFalling;
    public float groundCheckLength;
    public float wallCheckLength;

    //private BoxCollider2D boxCollider;
    [SerializeField] private BoxCollider2D leftSideBoxCollider;
    [SerializeField] private BoxCollider2D rightSideBoxCollider;
    [SerializeField] private BoxCollider2D lowerBoxCollider;
    private SpriteRenderer spriteRenderer;
    private CharacterControl characterControl;

    [SerializeField] private LayerMask platformLayerMask;
    private void Start()
    {
        onGround = false;
        onWall = false;
        groundCheckLength = 0.2f;
        wallCheckLength = 0.1f;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterControl = GetComponent<CharacterControl>();
    }
    private void Update()
    {
        if (!CheckFall())
        {
            CheckRun();
            CheckGround();
        }
        CheckWall();
    }
    private bool CheckFall()
    {
        if(( !onWall || !onGround ) && rigidbody.velocity.y < -0.1f)
        {
            if(!isFalling)
            {
                isFalling = true;
                animator.SetTrigger("fall");
            }
        }
        else
        {
            isFalling = false;
        }
        return isFalling;
    }
    private void CheckRun()
    {
        if (rigidbody.velocity.x > 0.1f)
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = false;
        }
        else if (rigidbody.velocity.x < -0.1f)
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = true;
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
    private bool CheckGround()
    {
        RaycastHit2D groundHit2D = Physics2D.BoxCast(lowerBoxCollider.bounds.center, lowerBoxCollider.bounds.size, 0f, Vector2.down, groundCheckLength, platformLayerMask);
        if(groundHit2D.collider != null)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        return onGround;
    }
    private void CheckWall()
    {
        if (onWall)
        {
            rigidbody.velocity = new Vector2( rigidbody.velocity.x, -1.0f );
        }
    }
}
