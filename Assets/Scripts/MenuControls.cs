using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuControls : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ForestPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
    


