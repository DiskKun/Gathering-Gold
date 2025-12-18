using UnityEngine;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    [SerializeField]PressurePlate unlockingPlate;
    [SerializeField]bool isOpen = false;
    Transform pivotPoint;

    private void Start()
    {
        pivotPoint = GetComponentInParent<Transform>();
    }
    private void Update()
    {
        if (unlockingPlate.CheckPlateActive())
            isOpen = true;
        else 
            isOpen = false;

        if (isOpen)
        {
            
            pivotPoint.eulerAngles = new Vector3(0, -90, 0);
        } else
        {
            pivotPoint.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
