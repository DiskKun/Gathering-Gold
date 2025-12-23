using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    [SerializeField]PressurePlate unlockingPlate;
    [SerializeField] PressurePlate unlockingPlateRamp2;
    
    //[SerializeField] Vector3 basePosition;
    [SerializeField] Vector3 baseRotation;
    //[SerializeField] Vector3 rampPosition;
    [SerializeField] Vector3 rampRotation;
    [SerializeField] Vector2 grandDoorRotation;
    [SerializeField] Vector3 grandDoorBaseRotation;
    [SerializeField] float lerpDuration = 3;
    [SerializeField] bool rampDown = false;
    [SerializeField] bool OverrideIsOpen;
    Vector3 basePosition;
    Vector3 currentPosition; 
    [SerializeField]bool isOpen = false;
  
    enum DoorTypes
    {
     door = 1,
     ramp = 2,
     granddoor =3
    }

    [SerializeField] DoorTypes doorTypes;

    
    [SerializeField]Transform pivotPoint;

    // stuff for sfx
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1;
    private bool rampSound = false;

    private void Start()
    {
        //pivotPoint = transform.parent.GetComponent<Transform>(); // broken rn. wont fidn transform in parent object. did a bypass
        rampDown = false;
        baseRotation = transform.parent.localEulerAngles;
        basePosition = transform.parent.position;
        grandDoorBaseRotation = transform.parent.localEulerAngles;  
    }
    private void Update()
    {
      
        switch (doorTypes)
        {
            case DoorTypes.door:
                if (unlockingPlate.CheckPlateActive())
                    isOpen = true;
                else
                    isOpen = false;

                if (isOpen)
                {

                    transform.parent.position += new Vector3(0,-10, 0) *Time.deltaTime;
                   currentPosition = transform.parent.position;
                }
                else
                {
                    transform.parent.position = basePosition;
                }
                break;
            case DoorTypes.ramp:
                if ((unlockingPlateRamp2.CheckPlateActive() && unlockingPlate.CheckPlateActive()) | OverrideIsOpen)
                {
                    isOpen = true;
                }
                else
                    isOpen= false;
                if (isOpen)
                {
                    StartCoroutine("LerpRamp");

                }
                else
                {
                 //   if(rampDown)
                  //      StartCoroutine("LerpRampUp");
                }
                break;
            case DoorTypes.granddoor:
                if((unlockingPlate.CheckPlateActive() && unlockingPlateRamp2.CheckPlateActive()) || OverrideIsOpen)
                {
                    StartCoroutine(LerpGrandDoor());
                }

                break;

        }

        if (rampSound == true)
        {
            // play sfx
            SoundManager.PlaySound(sound, volume);
            rampSound = false;
        }
    }
    IEnumerator LerpGrandDoor()
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.parent.eulerAngles = Vector3.Lerp(grandDoorBaseRotation, grandDoorRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.parent.eulerAngles = grandDoorRotation;
    }
    IEnumerator LerpRamp()
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.parent.eulerAngles = Vector3.Lerp(baseRotation, rampRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.parent.eulerAngles = rampRotation;
        rampDown = true;
        rampSound = true;
    }
    IEnumerator LerpRampUp()
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.parent.eulerAngles = Vector3.Lerp(rampRotation, baseRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.parent.eulerAngles = baseRotation;
        rampDown = false;
    }

}
