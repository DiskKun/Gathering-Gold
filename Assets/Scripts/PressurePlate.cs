using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Activation Info")]
    [SerializeField] bool plateActivated = false;
    [SerializeField] float activationWeight = 5;

    [SerializeField] float totalWeightOnPlate = 0;

    [Header("Detection Variables")]
    [SerializeField] float detectOffset = 0.2f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask lootLayer;
    [SerializeField]Vector3 detectArea;
    [SerializeField]Collider[] collidersOnPlate;

    private void Start()
    {
        detectArea = new Vector3(transform.position.x, transform.position.y + detectOffset, transform.position.z); //sets zone for detecting weight.
    }

    // method called by mechanism this is locked by.
    public bool CheckPlateActive()
    {
        if (totalWeightOnPlate >= activationWeight)
        {
            plateActivated = true;
            return true;
        }
        plateActivated = false;
        return false;
    }

    // so it doesn't calculate when not needed, collider check only works while objects are on it's trigger zone.
    private void OnTriggerEnter(Collider other)
    {
        // upon any collider entering the pressure plate's trigger zone, begins checking what's making contact with plate floor.
        collidersOnPlate = Physics.OverlapBox(detectArea, transform.localScale, Quaternion.identity, playerLayer | lootLayer); // stores an array of ALL objects WITH the Player and Loot layer. This is my solution to be able to count them all
        //Debug.Log(collidersOnPlate.Length); // commented out but not removed cuz i need this every so often lol.
        if(collidersOnPlate.Length > 0)
        {
            CalculateTotalMass();
        }
    }

    void CalculateTotalMass()
    {
        totalWeightOnPlate = 0f; // resets total for new calculation.

        foreach(Collider collider in collidersOnPlate)
        {
            CarryWeight CarryWeight = collider.gameObject.GetComponent<CarryWeight>(); // finds and gets carry weight script of incoming object. only objects with the CarryWeight script will do things.
            totalWeightOnPlate += CarryWeight.playerWeight/2; // adds weight to variable. (compounds for each object). !Need to /2 because character controllers have their own colliders and i dont wanna touch the player config.
            
        }
        Debug.Log("This weight on plate: " + totalWeightOnPlate);
    }

    private void OnTriggerExit(Collider other)
    {
        totalWeightOnPlate = 0f; // resets balance to 0
    }
    private void FixedUpdate()
    {
        Animator animator = GetComponent<Animator>();
        // I set up the animator to run based on Descend Stage. 0 is fully unnaffected, 1 is 1/3rd down, 2 is 2/3rd down, and 3 is fully down.

        if(totalWeightOnPlate >= activationWeight) // if at or over activation weight, 
        {
            animator.SetInteger("Descend Stage", 3); // fully descend
            animator.SetInteger("Ascend Stage", 0); // and block ascending.
        }
        else if (totalWeightOnPlate >= (activationWeight + activationWeight)/3) // if at or over 2/3rd the activation weight
        {
            animator.SetInteger("Descend Stage", 2); // descend 2/3rd
            animator.SetInteger("Ascend Stage", 1); // or ascend 1/3rd.
        }
         else if (totalWeightOnPlate >= activationWeight/3) // if at or over 1/3rd of activation weight,
        {
            animator.SetInteger("Descend Stage", 1); //  descend 1/3rd
            animator.SetInteger("Ascend Stage", 2); // or ascend 2/3rd.
        }
         else if (totalWeightOnPlate < activationWeight/3) // if less than 1/3rd of activation weight,
        {
            animator.SetInteger("Descend Stage", 0); // fully hold down.
            animator.SetInteger("Ascend Stage", 3); // or ascend fully.
        }

        // checks if plate is active
        CheckPlateActive();
    }

    private void OnDrawGizmos() // to see where the detect area is in the scene.
    {
        if (collidersOnPlate.Length > 0) // debug colors
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(detectArea, transform.localScale);
    }
}
