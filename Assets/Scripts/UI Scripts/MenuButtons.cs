using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    /* CN's Scene Order
     * 0: Main Menu
     * 1: Game Scene
     * 2: End Menu
     * 3: Settings
     */

    // called by Play button at MAIN MENU.
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // loads the scene positioned next to it in the build menu (which should be the game.)
    }

    // called by Replay button at END MENU.
    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // loads the scene positioned behind it in the build menu (which should be the game).
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0); // goes straight back to main menu.
    }

    // Called by Exit button in ANY SCENE. Quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }
}
