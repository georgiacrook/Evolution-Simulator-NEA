using UnityEngine;

public class FoxMovement : MonoBehaviour
{
    private float moveSpeed = 30f;
    public float raycastHeight = 10f;
    public float raycastDistance = 20f;
    public float terrainOffset = 0.5f; //keeps organism slightly above ground
    public float checkDistance = 1f; //checking for water or terrain
    public LayerMask groundLayer;
    public LayerMask waterLayer;
    float oldAngle = 180;
    private Vector3 moveDirection;

    public Animator animator;
    public Transform target;
    public float chaseRange = 70f;
    private Vector3 lastPosition;
    public float speed;
    public bool isChasing = false;
    public bool eat = false;

    void Start()
    {
        PickNewDirection();

        lastPosition = transform.position;

        if (animator == null)
        {
            animator = GetComponent<Animator>(); //retrieve the object 
        }
    }

    void Update()
    {
        if (!IsGroundAhead() || IsWaterAhead()) //if reached edge of world or water
        {
            PickNewDirection();
        }

        transform.position += moveDirection * moveSpeed * Time.deltaTime; //go forward

        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + raycastHeight, transform.position.z); //makes sure raycast doesn't hit collider

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance, groundLayer)) //if ground ahead
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + terrainOffset; //organism is slightly above terrain
            transform.position = pos;
        }

        RabbitCheck();


        // Animator updates
        animator.SetFloat("speed", speed);
        animator.SetBool("isChasing", isChasing);
        animator.SetBool("eat", eat);
    }

    void PickNewDirection()
    {
        float angle = Random.Range(0f, 360f);

        while (angle == oldAngle)
        {
            angle = Random.Range(0f, 360f);
        }

        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);


        oldAngle = angle;
        moveDirection = new Vector3(x, 0f, z).normalized; //normalized ensures object moves at constant speed
        Quaternion lookRot = Quaternion.LookRotation(moveDirection);
    }

    bool IsGroundAhead()
    {
        Vector3 checkPos = transform.position + moveDirection * checkDistance + Vector3.up * raycastHeight; //current position of organism
        return Physics.Raycast(checkPos, Vector3.down, raycastDistance, groundLayer); //returns true if groundlayer is found
    }

    bool IsWaterAhead()
    {
        Vector3 checkPos = transform.position + moveDirection * checkDistance + Vector3.up * 0.5f; //current position of organism
        return Physics.Raycast(checkPos, Vector3.down, 1f, waterLayer); //returns true if waterlayer is found
    }

    void RabbitCheck()
    {
        speed = Vector3.Distance(lastPosition, transform.position) * 100f;
        lastPosition = transform.position;

        isChasing = false;

        if (target == null)
        {
            // find nearest rabbit automatically if none set
            GameObject[] rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
            float nearest = Mathf.Infinity;
            Transform nearestRabbit = null;

            foreach (var r in rabbits)
            {
                float d = Vector3.Distance(transform.position, r.transform.position);
                if (d < nearest)
                {
                    nearest = d;
                    nearestRabbit = r.transform;
                }
            }

            if (nearestRabbit != null && nearest <= chaseRange)
            {
                target = nearestRabbit;
            }
        }

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= chaseRange)
            {
                isChasing = true;

                // fixed chase speed
                speed = 40f;

                // rotate smoothly toward rabbit
                Vector3 dir = (target.position - transform.position).normalized;
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);

                // move forward
                transform.position += transform.forward * speed * Time.deltaTime;
                distance = Vector3.Distance(transform.position, target.position); //new distance

                // if close enough → "catch" and destroy rabbit
                if (distance < 5.0f)
                {
                    eat = true;

                    Destroy(target.gameObject);
                    target = null;
                    isChasing = false;
                }
            }
            else
            {
                target = null;
            }
        }
    }
}
