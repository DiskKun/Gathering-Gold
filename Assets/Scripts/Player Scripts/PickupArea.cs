using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class PickupArea : MonoBehaviour
{
    Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerControls>().heldItemQueue = rb; // Queue this item for being picked up by the player
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerControls>().heldItemQueue = null;
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
        
    }
}
