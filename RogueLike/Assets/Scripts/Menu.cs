using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject[] menuPages;

    public void Start()
    {
        LoadPage(0);
    }

    public void LoadPage(int p)
    {
        foreach(GameObject page in menuPages)
        {
            page.SetActive(false);
        }
        menuPages[p].SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
