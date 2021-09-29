using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRedirection : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame() {       
        if(!File.Exists(DataManager.path)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }

    public void QuitGame() {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void Adopt() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }
}
