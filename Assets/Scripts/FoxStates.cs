using UnityEngine;

public class FoxStates : OrganismStates
{
    public Animator animator;
    public GameObject foxPrefab;

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

        //make it so that it only prints debug log if there isn't too many debug logs already printed
        //Debug.Log("Speed: " + speed.ToString("F2"));
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

        if (lifespanLength == 60) // 1 minute
        {
            Debug.Log($"Germination reached: {this.gameObject.name}");
            Vector3 position = organism.transform.position;
            GameObject fox = Instantiate(foxPrefab, position, Quaternion.identity); //creates clones of the fox
        }
    }
}