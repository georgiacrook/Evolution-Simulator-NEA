using UnityEngine;

public class FoxAnimationSwitching : MonoBehaviour
{
    public Animator animator;
    public Transform target;
    public float chaseRange = 50f;

    private Vector3 lastPosition;
    public float speed;
    public bool isChasing = false;

    //protected OrganismStates states;

    void Start()
    {
        //states = GetComponent<OrganismStates>();

        lastPosition = transform.position;

        if (animator == null) 
        {
            animator = GetComponent<Animator>(); //retrieve the object 
        }
    }

    void Update()
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
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f);

                // move forward
                transform.position += transform.forward * speed * Time.deltaTime;

                // if close enough → "catch" and destroy rabbit
                if (distance < 1.0f)
                {
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

        // Animator updates
        animator.SetFloat("Speed", speed);
        Debug.Log($"Name: fox | Speed: {speed}");
        animator.SetBool("IsChasing", isChasing);
        Debug.Log($"fox is chasing");
    }
}
