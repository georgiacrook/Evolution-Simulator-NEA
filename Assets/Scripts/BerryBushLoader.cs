using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BerryBushLoader : OrganismLoader
{
    public GameObject berryBushPrefab;
    public TMP_Text berryCount;

    private List<GameObject> bushes = new List<GameObject>(); //list of loaded bushes

    void Start()
    {
        int count = PlayerPrefs.GetInt("BerryCountSlider", 10); //references the slider amount the user chose
        Spawn(count);

        berryCount.text = ($"Berry Bush Count: {count.ToString()} ");
    }

    public void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = base.GetRandomPosition();
            GameObject bush = Instantiate(berryBushPrefab, position, Quaternion.identity); //creates clones of the berry bush
            bushes.Add(bush);
        }
    }
}
