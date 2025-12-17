using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // called by Play button at Main Menu
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // loads the scene positioned next to it in the build menu
    }
    // Called by Exit button. Quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }
}
