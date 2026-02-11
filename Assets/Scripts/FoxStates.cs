using UnityEngine;

public class FoxStates : OrganismStates
{
    public Animator animator;
    public GameObject foxPrefab;
    private bool hasGerminated = false;

    protected override void Start()
    {
        base.Start();


        if (animator == null)
        {
            animator = GetComponent<Animator>(); //retrieve the object 
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Eat()
    {
        if (animator.GetBool("eat") == true)
        {
            base.Eat();
        }
    }

    protected override void Lifespan()
    {
        base.Lifespan();

        if (lifespanLength >= 60 && !hasGerminated)
        {
            hasGerminated = true;

            if (Random.value <= 0.5f)
            {
                FoxLoader loader = FindAnyObjectByType<FoxLoader>();
                if (loader != null)
                {
                    Movement parentMovement = GetComponent<Movement>();
                    float speed = parentMovement != null ? parentMovement.moveSpeed : 30f;
                    loader.SpawnOffspring(organism.transform.position, speed, vision);
                    Debug.Log($"{this.gameObject.name} spawned 1 offspring");
                }
            }
            else
            {
                Debug.Log($"{this.gameObject.name} did not reproduce");
            }
        }
    }

    protected override void Hungry()
    {
        //dont eat berries
    }
}