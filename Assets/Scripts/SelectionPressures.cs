using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPressures: MonoBehaviour
{
    public Texture2D newColour;
    public bool disease = false;
    public Vector2 location;

    //prefabs & gameObjects
    //GameObject fox;
    //fox.GetComponent<FoxLoader>();
    public GameObject foxPrefab;
    public GameObject berry;

    public FoxLoader FoxLoaderScript;


    public void Disease()
    {
        Cursor.SetCursor(newColour, location, CursorMode.Auto);
        Debug.Log("Cursor colour changed");
        disease = true;
    }

    public void Predation(int count)
    {
        //add in (count) amount of foxes
        FoxLoader loader = FindAnyObjectByType<FoxLoader>();
        if (loader != null)
        {
            StartCoroutine(loader.Spawn(count)); 
            Debug.Log($"running spawn() {count}");
        }
    }

    public void FoodLevels(int change)
    {
        //add/remove (change) amount of berries

        if (change < 0) // destroy berries
        {
            GameObject[] berries = GameObject.FindGameObjectsWithTag("Berry");
            int toDestroy = Mathf.Min(-change, berries.Length); // don't destroy more than exist
            for (int i = 0; i < toDestroy; i++)
            {
                Destroy(berries[i]);
            }
        }
        else // add new berries
        {
            BerryBushLoader loader = FindAnyObjectByType<BerryBushLoader>();
            if (loader != null)
            {
                loader.Spawn(change);
            }
        }
    }
}
