using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D body;
    public float speed = 3;
    public float acceleration = 1.1f;
    [Range(0f, 1f)]
    public float groundDecay;
    public BoxCollider2D groundCheck;

    public LayerMask groundMask;
    float xInput;
    float yInput;

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

    }

    void FixedUpdate()
    {
        ApplyFriction();
        
    }

    void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void moveWithInput()
    {
        if (Mathf.Abs(xInput) > 0)
        {

            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(body.linearVelocity.x + increment, -speed, speed);

            body.linearVelocity = new Vector2(newSpeed, body.linearVelocity.y);

            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1, 1);
        }


    }

    void ApplyFriction()
    {
        if ( xInput == 0)
        {
            body.linearVelocity *= groundDecay;
        }
    }
}

