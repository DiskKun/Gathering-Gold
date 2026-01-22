using UnityEngine;

public class AnimationControlss : MonoBehaviour
{
    public Animator ani;
    [SerializeField]
    public bool IsWalking = false;
    [SerializeField]
    public bool IsGrounded;
    [SerializeField]
    public bool hasObject;
    [SerializeField]
    public bool IdlePickUp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWalking)
            ani.SetBool("IsWalking", true); 
        else
            ani.SetBool("IsWalking", false);

        if (hasObject)
            ani.SetBool("hasObject", true);
        else
            ani.SetBool("hasObject", false);
    }

   
}
