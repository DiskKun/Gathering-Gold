using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class PickupArea : MonoBehaviour
{
    Rigidbody rb;

    public enum PickupWeight {
        light,
        heavy
    }
    [SerializeField] PickupWeight itemWeight;

    public bool waitingForOtherPlayer;
    [SerializeField]PlayerControls firstPlayer;
    [SerializeField] PlayerControls secondPlayer;
    [SerializeField] float cutoffDistance = 5f;



    private void OnTriggerEnter(Collider other)
    {
        switch (itemWeight) // reads for item weight case types
        {
            case PickupWeight.light: // if object is light
                if (other.tag == "Player")
                {
                    other.GetComponent<PlayerControls>().heldItemQueue = rb; // Queue this item for being picked up by the player
                }
                break;
            case PickupWeight.heavy:
                if(other.tag == "Player") // if player enters
                {
                    if (waitingForOtherPlayer == false) // and there is no one waiting
                    {
                        transform.parent.position = transform.position + Vector3.up / 4;
                        rb.useGravity = false; // lift affordance
                        waitingForOtherPlayer = true; // wait for other player
                        firstPlayer = other.GetComponent <PlayerControls>(); // grabs and stores first player's control script
                    }
                    else if (waitingForOtherPlayer == true)
                    {
                        secondPlayer = other.GetComponent <PlayerControls>(); // stores second player's control script

                        if (firstPlayer != null && secondPlayer != firstPlayer)
                            AssignBothPlayersHeavyObjectRB();
                        else
                            Debug.LogError("no other player");
                    } 
                }
                break;

        }
    }
    void AssignBothPlayersHeavyObjectRB()
    {
        firstPlayer.heldHeavyItemQueue = rb;
        secondPlayer.heldHeavyItemQueue = rb;
        rb.useGravity = true; // re-enable gravity
        Debug.Log("assigned heavy object to both players");
    }
    void CalculateCurrentMidpoint()
    {
        Vector3 playerMidpoint = (firstPlayer.transform.position + secondPlayer.transform.position) / 2;
        firstPlayer.playerMidpoint = playerMidpoint;
        secondPlayer.playerMidpoint = playerMidpoint;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerControls>().heldItemQueue = null;
            rb.useGravity = true;
            waitingForOtherPlayer = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemWeight == PickupWeight.light)
            return;
        if (secondPlayer == null)
            return;

        //drops it for both players if either player decide to drop it.
        if (firstPlayer.dropForBothPlayers | secondPlayer.dropForBothPlayers)
        {
            DropProcedure();
        }

        if (Vector3.Distance(firstPlayer.transform.position, secondPlayer.transform.position)>=cutoffDistance)
        {
            DropProcedure();
        }

        // makes it so that if one player carries, both do.
        if (firstPlayer.heldHeavyItemRB == rb)
        {
            secondPlayer.heldHeavyItemRB = rb;
            secondPlayer.heldHeavyItemQueue = null;
        }
        else if (secondPlayer.heldHeavyItemRB == rb)
        {
            firstPlayer.heldHeavyItemRB = rb;
            firstPlayer.heldHeavyItemQueue = null;
        }

        if(firstPlayer.heldHeavyItemRB == rb | secondPlayer.heldHeavyItemRB == rb)
        {
            CalculateCurrentMidpoint();
        }
    }
    void DropProcedure()
    {
        firstPlayer.heldHeavyItemRB = null;
        secondPlayer.heldHeavyItemRB = null;
        firstPlayer.dropForBothPlayers = false;
        secondPlayer.dropForBothPlayers = false;
    }
}
