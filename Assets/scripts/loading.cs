using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loading : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        AsyncOperation assynload = SceneManager.LoadSceneAsync(2);

        while (!assynload.isDone)
        {
            yield return null;
        }
    }
}
