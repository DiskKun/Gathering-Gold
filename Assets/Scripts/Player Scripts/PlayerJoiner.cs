using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class PlayerJoiner : MonoBehaviour
{
    public PlayerInput playerOne;
    public PlayerInput playerTwo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerOne.SwitchCurrentControlScheme("Keyboard 1", Keyboard.current);
        playerTwo.SwitchCurrentControlScheme("Keyboard 2", Keyboard.current);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
