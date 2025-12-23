using System.Collections;
using Unity.VisualScripting;
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
            StartCoroutine(EndSequenceDelay(5,0));
        }

        // checks for win condition in Drop Off point
        if (CheckForWinCon())
        {
            StartCoroutine(EndSequenceDelay(7, 1));
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
    IEnumerator EndSequenceDelay(float delay, int endCaseInt)
    {
        yield return new WaitForSeconds(delay);
        ResetGame();
        switch (endCaseInt)
        {
            case 0: // loss case
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // changes scene to Loss Screen
                break;
            case 1: // win case
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); // changes scene to Win Screen
                break;

        }
    }

    void ResetGame()
    {
        // insert whatever things need to reset here, if any.
    }
}
