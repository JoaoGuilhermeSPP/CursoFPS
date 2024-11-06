using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{

    public GameObject gameOver;
    public GameObject win;
    public static Controller gc;

    private void Start()
    {
        gc = this;
    }

    public void ShowGameOver() 
    {
        gameOver.SetActive(true);
        Invoke("Menu", 2f);

    }

    public void ShowWin() 
    {
        win.SetActive(true);
        Invoke("Menu", 3f);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

}
