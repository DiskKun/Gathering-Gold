using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]

    [Header("Loss Condition")]
    [SerializeField]
    Dragon CheckThisDragon;
   
    [Header("Win Condition")]
    [SerializeField]
    TreasureDropoff CheckThisDropOff;



    private void Update()
    {
        //checks for loss condition in linked Dragon
        if (CheckForLossCon())
        {
            ResetGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // changes scene to Loss Screen
        }

        // checks for win condition in Drop Off point
        if (CheckForWinCon())
        {
            ResetGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); // changes scene to Win Screen
        }
    }
    bool CheckForLossCon()
    {
        if (CheckThisDragon == null)
        {
            Debug.LogError("No Dragon Attached to GM");
            return false;
        }
        if (!CheckThisDragon.asleep)
            return true;
        else return false;
    }
    bool CheckForWinCon()
    {
        if (CheckThisDropOff == null)
        {
            Debug.LogError("No Dropoff Attached to GM");
            return false;
        }
        if (CheckThisDropOff.drive)
            return true;
        else return false;
    }

    void ResetGame()
    {

    }
}
