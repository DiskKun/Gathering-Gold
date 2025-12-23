using UnityEngine;

public class AnimationControlss : MonoBehaviour
{
    public Animator ani;
    private bool IsWalking = false;
    private bool IsGrounded;
    private bool hasObject;
    private bool IdlePickUp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
        //walking works, need to make it stop
        if (Input.GetKeyDown(KeyCode.A))
        {
            ani.SetBool("IsWalking", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            ani.SetBool("IsWalking", false);
        }
        
        //pick up works
        if (Input.GetKey(KeyCode.S))
        {
            hasObject = true;
            ani.SetTrigger("PickUp");
            
            //ani.ResetTrigger("PickUp");
            //idle while holding object
            if (hasObject == true)
            {
                ani.SetBool("IdlePickUp", true);
            }
           
        }
        //put down object
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("working");
            ani.SetTrigger("PutDown");
            hasObject = false;
            
        }





        //jump works
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ani.SetTrigger("Jump");
        }


        //die works
        if (Input.GetKeyDown(KeyCode.D))
        {
            ani.SetTrigger("Die");
        }


    }

   
}
