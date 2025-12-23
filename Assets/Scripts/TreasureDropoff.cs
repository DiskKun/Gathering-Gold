using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureDropoff : MonoBehaviour
{
    [Tooltip("Camera manager reference")]
    [SerializeField]
    CameraManager cameraManager;
    [Tooltip("Whether or not players must be in the dropoff area to send off the treasure")]
    [SerializeField]
    bool requirePlayersInArea = false;
    [SerializeField]
    [Tooltip("The amount of treasure that must be in the area for level completion")]
    int treasureThreshold = 1;
    [SerializeField]
    [Tooltip("Speed at which the cart will drive away")]
    float driveSpeed;
    [SerializeField]
    [Tooltip("Scene reference to the single item required to complete the level. Ignores TreasureThreshold if set.")]
    GameObject requiredItem;

    int treasureInArea = 0; // the # of treasures currently detected in the area
    int playersInArea = 0; // the # of players currently detected in the area
    public bool drive = false; // whether or not the cart is driving
    Transform visuals; // the empty gameobject that all cart visuals should be under
    List<GameObject> objectsInDropoff = new List<GameObject>(); // list of objects currently being detected in the area
    List<GameObject> driveObjects = new List<GameObject>(); // list of objects to drive away with - can't change once the cart starts driving

    private void Start()
    {
        visuals = transform.GetChild(0); // get a reference to the visuals transform for driving
    }

    private void OnTriggerEnter(Collider other)
    {
        // keep track of objects in the dropoff area
        if (other.tag == "Treasure")
        {
            treasureInArea += 1;
            objectsInDropoff.Add(other.gameObject);
        }
        if (other.tag == "Player")
        {
            playersInArea += 1;
            objectsInDropoff.Add(other.gameObject);

        }


        // check to see if the drive conditions are met, if so, run the TreasureSendoff method
        if (!drive)
        {


            if (requirePlayersInArea)
            {
                if (requiredItem)
                {
                    if (objectsInDropoff.Contains(requiredItem) && playersInArea == 2)
                    {
                        TreasureSendoff();
                    }
                } else if (playersInArea == 2 && treasureInArea == treasureThreshold)
                {
                    TreasureSendoff();
                }
            }
            else
            {
                if (requiredItem)
                {
                    if (objectsInDropoff.Contains(requiredItem))
                    {
                        TreasureSendoff();
                    }
                }
                if (treasureInArea == treasureThreshold)
                {
                    TreasureSendoff();
                }
            }


        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Treasure")
        {
            treasureInArea -= 1;
            objectsInDropoff.Remove(other.gameObject);
        }
        if (other.tag == "Player")
        {
            playersInArea -= 1;
            objectsInDropoff.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        // drive the cart and objects in the cart away
        if (drive)
        {
            foreach (GameObject g in driveObjects)
            {
                g.transform.position += Vector3.right * Time.deltaTime * driveSpeed;
            }
            visuals.position += Vector3.right * Time.deltaTime * driveSpeed;
        }
    }

    void TreasureSendoff()
    {
        cameraManager.camIndex = 1; // switch the camera to the cart cam
        StartCoroutine(Drive());

    }

    IEnumerator Drive()
    {
        yield return new WaitForSeconds(1); // wait 1 second for the camera to switch
        foreach (GameObject g in objectsInDropoff)
        {
            driveObjects.Add(g); // create frozen list of objects in the cart at time of completion
        }
        foreach (GameObject g in driveObjects)
        {
            PlayerControls p;
            if (g.TryGetComponent<PlayerControls>(out p))
            {
                p.enabled = false; // turn off the player controllers so that the players can't move while the cart is driving
            }
        }
        drive = true; // start driving
    }

}
