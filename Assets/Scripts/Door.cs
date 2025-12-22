using UnityEngine;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    [SerializeField]PressurePlate unlockingPlate;
    [SerializeField] PressurePlate unlockingPlateRamp2;

    [SerializeField]bool isOpen = false;
  
    enum DoorTypes
    {
     door = 1,
     ramp = 2
    }

    [SerializeField] DoorTypes doorTypes;

    
    Transform pivotPoint;


    
    private void Start()
    {
        pivotPoint = GetComponentInParent<Transform>();
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

                    pivotPoint.eulerAngles = new Vector3(0, -90, 0);
                }
                else
                {
                    pivotPoint.eulerAngles = new Vector3(0, 0, 0);
                }
                break;
            case DoorTypes.ramp:
                if (unlockingPlateRamp2.CheckPlateActive() && unlockingPlate.CheckPlateActive())
                {
                    isOpen = true;
                }
                if (isOpen)
                {

                    pivotPoint.eulerAngles += new Vector3(-137.619f, 77.944f, -37.21301f);
                 
                }
                else
                {
                    pivotPoint.eulerAngles = new Vector3(0, 0, 0);
                }
                break;

        }
    }
    
}
