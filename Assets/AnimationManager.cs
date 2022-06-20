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
        groundCheckLength = 0.4f;
        wallCheckLength = 0.1f;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterControl = GetComponent<CharacterControl>();
    }
    private void Update()
    {
        GroundAndWall();
        CheckFall();
        CheckRun();
    }
    private void CheckFall()
    {
        if(( !onWall || !onGround ) && rigidbody.velocity.y < 0)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
        }
        else
        {
            isFalling = false;
            animator.SetBool("isFalling", false);
        }
    }

    private void CheckRun()
    {
        if (rigidbody.velocity.x > 0.0f)
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = false;
        }
        else if (rigidbody.velocity.x < -0.0f)
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = true;
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
    private void GroundAndWall()
    {
        RaycastHit2D groundHit2D = Physics2D.BoxCast(lowerBoxCollider.bounds.center, lowerBoxCollider.bounds.size, 0f, Vector2.down, groundCheckLength, platformLayerMask);
        RaycastHit2D leftWallHit2D = Physics2D.BoxCast(leftSideBoxCollider.bounds.center, leftSideBoxCollider.bounds.size, 0f, Vector2.left, wallCheckLength, platformLayerMask);
        RaycastHit2D rightWallHit2D = Physics2D.BoxCast(rightSideBoxCollider.bounds.center, rightSideBoxCollider.bounds.size, 0f, Vector2.right, wallCheckLength, platformLayerMask);

        if((leftWallHit2D.collider != null || rightWallHit2D.collider != null ) && groundHit2D.collider == null)
        {
            onWall = true;
            if(rightWallHit2D.collider != null && rigidbody.velocity.x > 0)
            {
                animator.SetTrigger("wall");
                characterControl.jumpCount = 0;
            }
            else if(leftWallHit2D.collider != null && rigidbody.velocity.x < 0)
            {
                animator.SetTrigger("wall");
                characterControl.jumpCount = 0;
            }
        }
        else
        {
            onWall = false;
        }

        if(groundHit2D.collider != null)
        {
            onGround = true;
            if(rigidbody.velocity.y < 0)
            {
                animator.SetTrigger("land");
                characterControl.jumpCount = 0;
            }
        }
        else
        {
            onGround = false;
        }
    }
}
