using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    private float speed;
    public float acc;
    public float breakAcc;
    private float jumpForce;
    public int jumpCount;

    private AnimationManager animationManager;

    private void Start()
    {
        jumpCount = 0;
        acc = 40f;
        breakAcc = 40f;
        speed = 5f;
        jumpForce = 10f;
        rigidbody = GetComponent<Rigidbody2D>();
        animationManager = GetComponent<AnimationManager>();
    }

    void Update()
    {
        // Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        HorizontalMove();
    }

    private void HorizontalMove()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if (rigidbody.velocity.x > -speed)
            {
                rigidbody.velocity = new Vector2(
                    Mathf.Clamp(rigidbody.velocity.x - acc * Time.deltaTime, -speed, speed),
                    rigidbody.velocity.y
                    );
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (rigidbody.velocity.x < speed)
            {
                rigidbody.velocity = new Vector2(
                    Mathf.Clamp(rigidbody.velocity.x + acc * Time.deltaTime, -speed, speed),
                    rigidbody.velocity.y
                    );
            }
        }
        else
        {
            if (Mathf.Abs(rigidbody.velocity.x) > 0.1f)
            {
                if(rigidbody.velocity.x > 0)
                {
                    rigidbody.velocity -= new Vector2( breakAcc * Time.deltaTime, 0.0f );
                }
                else
                {
                    rigidbody.velocity += new Vector2( breakAcc * Time.deltaTime, 0.0f );
                }
            }
            else
            {
                rigidbody.velocity = new Vector2(0.0f, rigidbody.velocity.y);
            }
        }
    }

    private void Jump()
    {
        switch(jumpCount)
        {
            case 0:
                if (animationManager.onWall)
                {
                    jumpCount++;
                    rigidbody.velocity = new Vector2(15.0f, jumpForce);
                    animationManager.animator.SetTrigger("jump");
                }
                else
                {
                    jumpCount++;
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                    animationManager.animator.SetTrigger("jump");
                }
                break;
            case 1:
                jumpCount++;
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                animationManager.animator.SetTrigger("doubleJump");
                break;
            default:
                break;

        }
    }
}
