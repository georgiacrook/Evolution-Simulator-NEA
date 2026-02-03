using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class OrganismStates : MonoBehaviour
{
    public GameObject organism;

    //attributes
    const float MAX_HUNGER = 100;
    const float MAX_THIRST = 100;

    public float hunger = 100f;
    public float thirst = 100f;
    private float hungerRate;
    private float thirstRate;

    //states
    protected bool isDrinking = false;
    protected bool isEating = false;

    //movement code
    protected Movement movement;
    public float speed = 70f;

    //layers
    public LayerMask waterLayer;
    public LayerMask foodLayer;
    public Transform food;
    public float vision;

    //time management
    public float lifespanLength;
    public float maxLifeLength = 300;

    public bool disease = false;
    private Vector3 lastPosition;

    //disease
    public Transform target;
    public float contaminationRange = 20f;

    protected virtual void Start()
    {
        movement = GetComponent<Movement>(); //movement code
        lastPosition = transform.position;

        hungerRate = Random.Range(0.006f, 0.012f);
        thirstRate = Random.Range(0.007f, 0.013f);

        vision = Random.Range(75f, 90f);
    }

    protected virtual void Update()
    {
        //speed calculation
        speed = Vector3.Distance(lastPosition, transform.position) * 100f;
        lastPosition = transform.position;

        Lifespan();

        //organism death
        if (hunger <= 0 || thirst <= 0)
        {
            Die();
        }

        hunger -= hungerRate;
        thirst -= thirstRate;

        if (hunger <= 75 && !isEating)
        {
            Hungry();
        }

        if (thirst <= 55 && !isDrinking)
        {
            Thirsty();
        }

        if (disease)
        {
            Diseased();
        }
    }

    public void Thirsty()
    {
        Debug.Log($" {this.gameObject.name} THIRSTY | Thirst: {thirst}");

        //check for water
        if (WaterCheck())
        {
            Drink();
        }
    }

    public void Hungry()
    {
        Debug.Log($"{this.gameObject.name} HUNGRY | Hunger: {hunger}");
        //check for food
        FoodCheck();
    }

    public bool WaterCheck()
    {
        Vector3 checkPos = transform.position + transform.forward * 1f + Vector3.up * 0.5f;
        return Physics.Raycast(checkPos, Vector3.down, 1f, waterLayer);
    }

    public void FoodCheck()
    {

        if (food == null)
        {
            // find nearest rabbit automatically if none set
            GameObject[] berries = GameObject.FindGameObjectsWithTag("Berry");
            float nearest = Mathf.Infinity;
            Transform nearestBerry = null;

            foreach (var b in berries)
            {
                float d = Vector3.Distance(transform.position, b.transform.position);
                if (d < nearest)
                {
                    nearest = d;
                    nearestBerry = b.transform;
                }
            }

            if (nearestBerry != null && nearest <= vision)
            {
                food = nearestBerry;
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, food.position);

            if (distance <= vision)
            {
                // rotate smoothly toward berry
                Vector3 dir = (food.position - transform.position).normalized;
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);

                // move forward
                transform.position += transform.forward * speed * Time.deltaTime; 

                if (distance < 5.0f)
                {
                    Eat();
                    food = null;
                }
            }
            else
            {
                food = null;
            }
        }
    }

    public void Drink()
    {
        isDrinking = true;

        thirst += 20f;
        if (thirst > MAX_THIRST) //caps thirst at 100
        {
            thirst = 100;
        }

        Debug.Log($"{this.gameObject.name} drank | Thirst: {thirst}");

        isDrinking = false;
    }

    protected virtual void Eat()
    {
        isEating = true;
        movement.enabled = false;

        hunger += 20f;
        if (hunger > MAX_HUNGER) //caps hunger at 100
        {
            hunger = 100;
        }

        Destroy(food.gameObject);
        Debug.Log($"{this.gameObject.name} ate | Hunger: {hunger}");

        movement.enabled = true;
        isEating = false;
    }

    protected virtual void Lifespan()
    {
        lifespanLength += Time.deltaTime;

        if (lifespanLength >= maxLifeLength) //5 minutes
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(organism);
        Debug.Log($"{this.gameObject.name} has died");
    }

    public void Diseased()
    {
        if (target == null)
        {
            // find nearest organism automatically if none set
            List<GameObject> organisms = new List<GameObject>();
            organisms.AddRange(GameObject.FindGameObjectsWithTag("Rabbit"));
            organisms.AddRange(GameObject.FindGameObjectsWithTag("Fox"));

            float nearest = Mathf.Infinity;
            Transform nearestOrganism = null;

            foreach (var r in organisms)
            {
                float d = Vector3.Distance(transform.position, r.transform.position);
                if (d < nearest)
                {
                    nearest = d;
                    nearestOrganism = r.transform;
                }
            }

            if (nearestOrganism != null && nearest <= contaminationRange)
            {
                target = nearestOrganism;
            }
        }

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= contaminationRange)
            {
                //pass on disease
                OrganismStates targetStates = target.GetComponent<OrganismStates>();

                if (targetStates != null)
                {
                    targetStates.disease = true;
                    Debug.Log($"{this.gameObject.name} passed on disease");
                }
            }
            else
            {
                target = null;
            }
        }
    }
}
