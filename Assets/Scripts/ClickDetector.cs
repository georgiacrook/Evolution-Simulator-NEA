using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ClickDetector : MonoBehaviour
{
    public Image panel;
    private Renderer _renderer;
    public TMP_Text nameText;
    public TMP_Text speedText;
    public TMP_Text lifespanText;
    public TMP_Text hungerText;
    public TMP_Text thirstText;
    public Image diseaseImage;


    public SelectionPressures SelectionPressuresScript;
    public OrganismStates OrganismStatesScript;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked");

        if (SelectionPressuresScript != null && SelectionPressuresScript.disease)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); //set cursor to normal 
            SelectionPressuresScript.disease = false;

            OrganismStatesScript.maxLifeLength = OrganismStatesScript.maxLifeLength / 2; //halve maxLifeLength
            Debug.Log($"Max lifespan for {this.gameObject.name}: {OrganismStatesScript.maxLifeLength}");

            OrganismStatesScript.disease = true;
        }
        else
        {
            panel.gameObject.SetActive(true);

            nameText.text = $"{this.gameObject.name}";
            speedText.text = "Speed: " + OrganismStatesScript.speed.ToString("F0");
            lifespanText.text = "Lifespan: " + OrganismStatesScript.lifespanLength.ToString("F0");
            hungerText.text = "Hunger: " + OrganismStatesScript.hunger.ToString("F0");
            thirstText.text = "Thirst: " + OrganismStatesScript.thirst.ToString("F0");

            diseaseImage.gameObject.SetActive(OrganismStatesScript.disease);
        }
    }
}

