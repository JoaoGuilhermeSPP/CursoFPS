using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MENU : MonoBehaviour
{
    private void Start()
    {
       Cursor.lockState = CursorLockMode.None; // Desbloqueia o cursor
        Cursor.visible = true; // Torna o cursor visível
    }
    public void Load()
    {
        SceneManager.LoadScene(1);
    }
    public void quit()
    {
        Application.Quit();
    }
}
