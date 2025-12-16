    using System.Collections.Generic;
    using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoxLoader : OrganismLoader
{
    public GameObject foxPrefab;
    public TMP_Text foxCount;

    public string organismName;
    private string[] organismNameList = { "Bob", "Pete", "Robert", "Linda", "Gertrude", "Olivia" };

    private int maxCount = 100;
    private int batchSize = 10;
    private static bool alreadySpawned = false; // scene-wide guard

    private List<GameObject> foxes = new List<GameObject>(); //list of loaded foxes

    void Start()
    {
        if (alreadySpawned) return;
        alreadySpawned = true;

        int requested = PlayerPrefs.GetInt("PredatorCountSlider", 10); //references the slider amount the user chose
        int count = Mathf.Clamp(requested, 0, maxCount);   // ← cap
        foxCount.text = ($"Fox Count: {count.ToString()} ");

        StartCoroutine(Spawn(count)); //start spawning gradually
    }

    public IEnumerator Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = base.GetRandomPosition();
            GameObject fox = Instantiate(foxPrefab, position, Quaternion.identity); //creates clones of the fox
            foxes.Add(fox);

            //give fox random name
            int randomIndex = Random.Range(0, organismNameList.Length);
            fox.name = organismNameList[randomIndex];

            if ((i + 1) % batchSize == 0) //every few spawns, yield to stop unity from crashing
            {
                yield return null; //waits one frame
            }
        }
    }
}