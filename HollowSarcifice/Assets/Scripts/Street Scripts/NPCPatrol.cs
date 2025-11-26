using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Transform currentTarget;
    private Transform spriteRoot;   // child with SpriteRenderer/Animator

    void Start()
    {
        currentTarget = pointB;
        spriteRoot = transform.Find("SpriteRoot");  // child object name
    }

    void Update()
    {
        // Move toward the current point
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            speed * Time.deltaTime
        );

        // Flip sprite based on direction
        if (currentTarget == pointB)
        {
            // moving right
            spriteRoot.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // moving left
            spriteRoot.localRotation = Quaternion.Euler(0, 0, 0);
        }

        // Switch direction when reaching the point
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }
}
