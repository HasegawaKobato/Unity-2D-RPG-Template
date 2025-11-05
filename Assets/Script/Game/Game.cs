using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputController.InputActions.Player.Enable();
        InputController.InputActions.UI.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
