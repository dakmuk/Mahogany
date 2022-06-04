using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    // 2022-06-04 테스트
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float speed;
    //private float acc = 10f;
    private float jumpForce;

    [SerializeField]
    private LayerMask platformLayerMask;

    [SerializeField]
    private BoxCollider2D lowerBoxCollider;

    [SerializeField]
    private int jumpCount;

    public bool onGround;
    public bool onWall;
    public bool isFalling;
    public float groundLength;
    public float wallLength;

    private void Start()
    {
        jumpCount = 0;
        onGround = false;
        groundLength = 0.4f;
        wallLength = 0.1f;
        speed = 5f;
        jumpForce = 10f;
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GroundAndWall();
        // Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        HorizontalMove(Input.GetAxis("Horizontal"));
        RunningCheck();
    }

    private void HorizontalMove(float value)
    {
        print(rigidbody.velocity);
        rigidbody.velocity = new Vector2(
            value * speed,
            rigidbody.velocity.y
            );
    }

    private void RunningCheck()
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

    private void Jump()
    {
        if (onGround)
        {
            jumpCount++;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            animator.SetTrigger("jump");
        }
        else if (jumpCount == 1)
        {
            jumpCount++;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            animator.SetTrigger("doubleJump");
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(lowerBoxCollider.bounds.center, lowerBoxCollider.bounds.size, 0f, Vector2.down, groundLength, platformLayerMask);
        if (raycastHit2D.collider != null)
        {
            if (rigidbody.velocity.y < 0 && isFalling)
            {
                print("land");
                isFalling = false;
                animator.SetBool("isFalling", isFalling);
                animator.SetTrigger("land");
                jumpCount = 0;
            }
            onGround = true;
        }
        else
        {
            if (rigidbody.velocity.y < 0f && !onWall)
            {
                isFalling = true;
                animator.SetBool("isFalling", isFalling);
            }
            onGround = false;
        }
    }

    private void WallCheck()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.left, wallLength, platformLayerMask);
        Color rayColor = Color.red;
        // DrawRay
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.left * (boxCollider.bounds.extents.x * 2), rayColor);
        //Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y), Vector2.down * (boxCollider.bounds.extents.y) * 2, rayColor);
        if (raycastHit2D.collider != null)
        {
            if(!onWall && !onGround)
            {
                print("wall");
                onWall = true;
                animator.SetTrigger("wall");
            }
        }
        else
        {
            onWall = false;
        }
    }
    
    private void GroundAndWall()
    {
        //RaycastHit2D wallHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.left, wallLength, platformLayerMask);
        Collider2D wallOverlap = Physics2D.OverlapBox(boxCollider.bounds.center, new Vector2(2 * (boxCollider.bounds.extents.x + wallLength), boxCollider.bounds.extents.y), 0.0f, platformLayerMask);
        RaycastHit2D groundHit2D = Physics2D.BoxCast(lowerBoxCollider.bounds.center, lowerBoxCollider.bounds.size, 0f, Vector2.down, groundLength, platformLayerMask);

        //if(wallHit2D.collider != null && groundHit2D.collider == null)
        if(wallOverlap != null && groundHit2D.collider == null)
        {
            onWall = true;
            if(Mathf.Abs(rigidbody.velocity.x) > 0)
            {
                animator.SetTrigger("wall");
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
                jumpCount = 0;
            }
        }
        else
        {
            onGround = false;
        }
    }
}
