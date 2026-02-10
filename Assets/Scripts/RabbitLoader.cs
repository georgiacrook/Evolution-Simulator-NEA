using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RabbitLoader : OrganismLoader
{
    public GameObject rabbitPrefab;
    public TMP_Text rabbitCount;

    public string organismName;
    private string[] organismNameList = { "Bob", "Pete", "Robert", "Linda", "Gertrude", "Olivia" };

    private int maxCount = 100;
    private int batchSize = 10;
    private static bool alreadySpawned = false; // scene-wide guard

    private List<GameObject> rabbits = new List<GameObject>(); //list of loaded rabbits

    void Start()
    {
        if (alreadySpawned) return;
        alreadySpawned = true;

        int requested = PlayerPrefs.GetInt("PreyCountSlider", 10); //references the slider amount the user chose
        int count = Mathf.Clamp(requested, 0, maxCount);   // ← cap
        rabbitCount.text = ($"Rabbit Count: {count.ToString()} "); ;
        
        StartCoroutine(Spawn(count)); //start spawning gradually
    }

    public IEnumerator Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = base.GetRandomPosition();
            GameObject rabbit = Instantiate(rabbitPrefab, position, Quaternion.identity); //creates clones of the rabbit
            rabbits.Add(rabbit);

            //give rabbit random name
            int randomIndex = Random.Range(0, organismNameList.Length);
            rabbit.name = organismNameList[randomIndex];

            if ((i + 1) % batchSize == 0) //every few spawns, yield to stop unity from crashing
            {
                yield return null; //waits one frame
            }
        }
    }

    public void SpawnOffspring(Vector3 position, float parentMoveSpeed, float parentVision, float parentDetectionRange, float parentRotationSpeed)
    {
        GameObject rabbit = Instantiate(rabbitPrefab, position, Quaternion.identity);
        rabbits.Add(rabbit);

        int randomIndex = Random.Range(0, organismNameList.Length);
        rabbit.name = organismNameList[randomIndex];

        Movement movement = rabbit.GetComponent<Movement>();
        if (movement != null)
        {
            movement.moveSpeed = parentMoveSpeed;
        }

        RabbitStates states = rabbit.GetComponent<RabbitStates>();
        if (states != null)
        {
            states.vision = parentVision;
            states.detectionRange = parentDetectionRange;
            states.rotationSpeed = parentRotationSpeed;
        }
    }
}