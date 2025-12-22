using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    [SerializeField]PressurePlate unlockingPlate;
    [SerializeField] PressurePlate unlockingPlateRamp2;
    
    //[SerializeField] Vector3 basePosition;
    [SerializeField] Vector3 baseRotation;
    //[SerializeField] Vector3 rampPosition;
    [SerializeField] Vector3 rampRotation;
    [SerializeField] float lerpDuration = 3;
    [SerializeField] bool rampDown = false;
    [SerializeField] bool OverrideIsOpen;


    [SerializeField]bool isOpen = false;
  
    enum DoorTypes
    {
     door = 1,
     ramp = 2
    }

    [SerializeField] DoorTypes doorTypes;

    
    [SerializeField]Transform pivotPoint;


    
    private void Start()
    {
        //pivotPoint = transform.parent.GetComponent<Transform>(); // broken rn. wont fidn transform in parent object. did a bypass
        rampDown = false;
        baseRotation = transform.parent.localEulerAngles;
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

                    transform.parent.eulerAngles = new Vector3(0, -90, 0);
                }
                else
                {
                    transform.parent.eulerAngles = new Vector3(0, 0, 0);
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
                    if(rampDown)
                        StartCoroutine("LerpRampUp");
                }
                break;

        }
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
