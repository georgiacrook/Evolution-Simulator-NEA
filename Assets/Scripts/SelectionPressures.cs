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
        //call FoxLoader with correct count (+10, +20, +30)
        FoxLoaderScript.Spawn(count); //doesn't run
        Debug.Log("running spawn()");
    }

    public void FoodLevels(int change)
    {
        //add/remove (change) amount of berries

        GameObject[] berries = GameObject.FindGameObjectsWithTag("Berry");

        if (change < 0) // destroy berries
        {
            int j = 0;
            for (int i = change; i < 0; i++)
            {
                Destroy(berries[j]);
                j++;
            }
        }
        else //add however many new berries
        {
            
        }
        
       
    }
}
