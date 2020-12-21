using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public Toggle[] mapSizes;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeMapSize()
    {
        Debug.Log("Changing map size");
        foreach (Toggle mapSize in mapSizes)
        {
            if (mapSize.isOn)
            {
                string s = mapSize.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
                PlayerPrefs.SetString("Map", s);
            }
        }
        Debug.Log("Toggle value " + PlayerPrefs.GetString("Map"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
