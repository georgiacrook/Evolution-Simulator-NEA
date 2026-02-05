using UnityEngine;

public class RabbitStates : OrganismStates
{
    public Animator animator;
    public GameObject rabbitPrefab;

    public float detectionRange = 55f;
    //public float runSpeed = 40f;
    public float rotationSpeed = 10f;

    public bool isRunning = false;
    private Movement movementScript;
    private bool hasGerminated = false;

    protected override void Start()
    {
        base.Start();
        if (movementScript == null)
        {
            movementScript = GetComponent<Movement>();
        }

        if (animator == null) 
        {
            animator = GetComponent<Animator>(); //retrieve the object 
        }
    }

    protected override void Update()
    {
        base.Update(); // hunger/thirst logic

        FoxCheck();

        //animator.SetFloat("Speed", speed);
        //animator.SetBool("isRunning", isRunning);
    }

    public bool FoxCheck()
    {
        GameObject[] foxes = GameObject.FindGameObjectsWithTag("Fox");
        Transform nearestFox = null;
        float nearestDist = 9999;

        foreach (var f in foxes) //retreives nearest fox by comparing distances
        {
            float distance = Vector3.Distance(transform.position, f.transform.position);
            if (distance < nearestDist)
            {
                nearestDist = distance;
                nearestFox = f.transform;
            }
        }

        if (nearestFox != null && nearestDist <= detectionRange)
        {
            isRunning = true;
            if (movementScript != null)
            {
                movementScript.isRunning = true;
            }

            // rotate away from fox
            Vector3 fleeDirection = (transform.position - nearestFox.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(fleeDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // move forward (manual)
            if (movementScript != null)
            {
                transform.position += transform.forward * (movementScript.moveSpeed + 15f) * Time.deltaTime;

            }

            return true;
        }
        else
        {
            isRunning = false;
            if (movementScript != null)
            {
                movementScript.isRunning = false;
            }
            return false;
        }

        //return false;
    }

    protected override void Lifespan()
    {
        base.Lifespan();

        if (lifespanLength == 60 && !hasGerminated) // 1 minute
        {
            hasGerminated = true;
            Debug.Log($"Germination reached: {this.gameObject.name}");
            Vector3 position = organism.transform.position;
            GameObject rabbit = Instantiate(rabbitPrefab, position, Quaternion.identity); //creates clones of the rabbit
        }
    }
}
