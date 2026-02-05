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
    private FoxStates foxStates;

    void Start()
    {
        moveSpeed = Random.Range(30f, 40f);  
        PickNewDirection();

        lastPosition = transform.position;
        foxStates = GetComponent<FoxStates>();

        if (animator == null)
        {
            animator = GetComponent<Animator>(); //retrieve the object 
        }
    }

    void Update()
    {
        DetectLocation();

        if (!IsGroundAhead() || IsWaterAhead()) //if reached edge of world or water
        {
            PickNewDirection();
        }

        if (!isChasing)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime; //go forward
        }

        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + raycastHeight, transform.position.z); //makes sure raycast doesn't hit collider

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance, groundLayer)) //if ground ahead
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + terrainOffset; //organism is slightly above terrain
            transform.position = pos;
        }

        bool isHungry = foxStates != null && foxStates.hunger <= 60;
        float chanceOfHunt = Random.Range(1f, 100f);

        if (isHungry || chanceOfHunt >= 75f)
        {
            RabbitCheck();
        }
        else if (!isHungry)
        {
            target = null;
            isChasing = false;
        }


        // Animator updates
        animator.SetFloat("speed", speed);
        animator.SetBool("isChasing", isChasing);
        animator.SetBool("eat", eat);
    }
    void DetectLocation()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeight;

        bool onWater = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, waterLayer);
        bool onGround = Physics.Raycast(rayOrigin, Vector3.down,  raycastDistance, groundLayer);

        if (onWater || !onGround)
        {
            float checkRadius = 10f;

            for (float dist = checkRadius; dist <= 50f; dist += 5f)
            {
                for (float angle = 0f; angle < 360f; angle += 45f)
                {
                    float x = Mathf.Cos(angle * Mathf.Deg2Rad) * dist;
                    float z = Mathf.Sin(angle * Mathf.Deg2Rad) * dist;

                    Vector3 checkPos = transform.position + new Vector3(x, raycastHeight, z);
                    RaycastHit hit;

                    bool foundGround = Physics.Raycast(checkPos, Vector3.down, out hit, raycastDistance, groundLayer);
                    bool foundWater = Physics.Raycast(checkPos, Vector3.down, raycastDistance, waterLayer);

                    if (foundGround && !foundWater)
                    {
                        // Teleport to valid position
                        transform.position = hit.point + Vector3.up * terrainOffset;
                        PickNewDirection();
                        return;
                    }
                }
            }            
        }
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
        moveDirection = new Vector3(x, 0f, z).normalized;
        Quaternion lookRot = Quaternion.LookRotation(moveDirection);
        transform.rotation = lookRot;
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
        isChasing = false;
        eat = false;

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

                speed = moveSpeed + 20f;

                // rotate smoothly toward rabbit
                Vector3 dir = (target.position - transform.position).normalized;
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);

                // update moveDirection to match chase direction
                moveDirection = transform.forward;

                // only move if ground ahead and no water
                if (IsGroundAhead() && !IsWaterAhead())
                {
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                else
                {
                    // can't follow into water/off map
                    target = null;
                    isChasing = false;
                }

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
