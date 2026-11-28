using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        GameState.ResetAll();
        SceneManager.LoadScene("Scene1_Home");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
