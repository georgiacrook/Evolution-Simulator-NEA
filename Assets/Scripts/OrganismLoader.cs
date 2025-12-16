using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrganismLoader : MonoBehaviour
{
    public Terrain terrain;

    public TMP_Text foxText;
    public TMP_Text rabbitText;
    public TMP_Text berryText;

    void Update()
    {
        GameObject[] foxes = GameObject.FindGameObjectsWithTag("Fox");
        int foxCount = foxes.Length;
        foxText.text = $"Foxes: {foxCount}";

        GameObject[] rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        int rabbitCount = rabbits.Length;
        rabbitText.text = $"Rabbits: {rabbitCount}";

        GameObject[] berries = GameObject.FindGameObjectsWithTag("Berry");
        int berryCount = berries.Length;
        berryText.text = $"Berries: {berryCount}";
    }

    //returns a random position on the terrain for the berry bush to spawn
    protected virtual Vector3 GetRandomPosition()
    {
        int maxAttempts = 100; //so the loop doesn't keep running
        float waterHeight = 20f; //in order for bushes to not spawn on water
        float maxSteepness = 22f; //in order for bushes to not spawn ontop of mountains

        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(0, terrainWidth);
            float z = Random.Range(0, terrainLength);
            float y = terrain.SampleHeight(new Vector3(x, 0, z));
            Vector3 worldPos = new Vector3(x, y, z);

            if (y < waterHeight)
                continue;

            Vector3 normal = terrain.terrainData.GetInterpolatedNormal(x / terrainWidth, z / terrainLength);
            float steepness = Vector3.Angle(Vector3.up, normal);
            if (steepness > maxSteepness)
                continue;

            return worldPos;
        }

        return new Vector3(terrainWidth / 2, terrain.SampleHeight(new Vector3(terrainWidth / 2, 0, terrainLength / 2)), terrainLength / 2);
    }

    
}
