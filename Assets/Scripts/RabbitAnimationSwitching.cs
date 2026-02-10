using UnityEngine;

public class RabbitAnimationSwitching : MonoBehaviour
{
    public Animator animator;
    public Transform target;
    public float chaseRange = 70f;

    private Vector3 lastPosition;
    public float speed;
    public bool isRunning = false;

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
        speed = Vector3.Distance(lastPosition, transform.position) * 100f; //calculating speed by using the distance between the last and current position of the object
        lastPosition = transform.position; //updating last position every run of update()

        isRunning = false;

        if (target == null)
        {
            // find nearest rabbit automatically if none set
            GameObject[] foxes = GameObject.FindGameObjectsWithTag("Fox");
            float nearest = Mathf.Infinity;
            Transform nearestFox = null;

            foreach (var f in foxes)
            {
                float d = Vector3.Distance(transform.position, f.transform.position);
                if (d < nearest)
                {
                    nearest = d;
                    nearestFox = f.transform;
                }
            }

            if (nearestFox != null && nearest <= chaseRange)
            {
                target = nearestFox;
            }
        }

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= chaseRange)
            {
                isRunning = true;

                // fixed chase speed
                speed = 50f;

                // rotate smoothly away from fox
                Vector3 dir = (transform.position - target.position).normalized;
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f);

                // move forward
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            else
            {
                target = null;
            }
        }

        // Animator updates
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", isRunning);
    }
}
