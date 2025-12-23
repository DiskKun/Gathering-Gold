using UnityEngine;
using System;
using Unity;

public class Dragon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the CameraManager")]
    CameraManager cameraManager;
    [SerializeField]
    [Tooltip("Particle System Reference")]
    ParticleSystem sleepParticles;
    [SerializeField]
    [Tooltip("Reference to the sleeping alert Icon")]
    GameObject alertIcon;
    [SerializeField]
    [Tooltip("The number of seconds that can be spent in the zone by the player(s) before the dragon wakes up")]
    float timeToWake = 10;
    [SerializeField]
    [Tooltip("Whether or not the timer goes into double-time when two players are in the area")]
    bool stackPlayerTime = false;
    [SerializeField]
    [Tooltip("The amount of time in seconds that the player has spent in the dragon's zone")]
    float secondsInZone = 0;

    [SerializeField]
    GameObject SleepingDragon;
    [SerializeField]
    GameObject AwakeDragon;

    int playersInArea;
    public bool asleep = true;


    // Update is called once per frame
    void Update()
    {
        MeshSwap(asleep);

        if (asleep)
        {
            if (stackPlayerTime)
            {
                secondsInZone += Time.deltaTime * playersInArea;
            }
            else if (playersInArea > 0)
            {
                secondsInZone += Time.deltaTime;
            }
            if (playersInArea == 0)
            {
                secondsInZone -= Time.deltaTime;
            }
            secondsInZone = Mathf.Clamp(secondsInZone, 0, timeToWake);
        }
        if (secondsInZone == 0 && !sleepParticles.isPlaying)
        {
            sleepParticles.Play();
            alertIcon.SetActive(false);
        }
        if (secondsInZone > 0 && sleepParticles.isPlaying)
        {
            sleepParticles.Stop();
            alertIcon.SetActive(true);
        }
        if (secondsInZone == timeToWake)
        {
            DragonWake();
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            playersInArea += 1;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playersInArea -= 1;
    }


    void MeshSwap(bool asleep)
    {
        switch (asleep)
        {
            case true:
                SleepingDragon.SetActive(true);
                AwakeDragon.SetActive(false);
                break;
            case false:
                SleepingDragon.SetActive(false);
                AwakeDragon.SetActive(true);
                break;
        }
    }

    void DragonWake()
    {
        asleep = false;
        alertIcon.SetActive(false);
        cameraManager.camIndex = 2;
    }
}
