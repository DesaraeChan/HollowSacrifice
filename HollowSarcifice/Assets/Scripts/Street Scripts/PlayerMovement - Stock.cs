using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D body;
    public float speed = 3;
    public float acceleration = 1.1f;
    [Range(0f, 1f)]
    public float groundDecay;
    public BoxCollider2D groundCheck;
    public Animator animator;

    public LayerMask groundMask;
    float xInput;

    public VectorValue startPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = startPosition.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        moveWithInput();
        ApplyFriction();
    }

    void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    void moveWithInput()
    {
        
        if (Mathf.Abs(xInput) > 0)
        {
            animator.SetBool("isWalking", true);

            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(body.linearVelocity.x + increment, -speed, speed);

            body.linearVelocity = new Vector2(newSpeed, body.linearVelocity.y);


            animator.SetFloat("InputX", xInput);
            animator.SetFloat("LastInputX", Mathf.Sign(xInput));
        } else{
            animator.SetBool("isWalking", false);
             
            }


    }

    void ApplyFriction()
    {
        if ( xInput < 0.2 && xInput > -0.2)
        {
             
            body.linearVelocity *= groundDecay;
        }
    }
}

