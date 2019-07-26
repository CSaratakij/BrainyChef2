using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    void Update()
    {
        //Hacks
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(0);
        }
    }

    public void ChangeToScene(int value)
    {
        SceneManager.LoadScene(value);
    }
}

