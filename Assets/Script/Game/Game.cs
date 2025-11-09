using UnityEngine;

public class Game : MonoBehaviour
{
    void Awake()
    {
        CardBattle.Main.Init();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputController.InputActions.Player.Enable();
        InputController.InputActions.UI.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CardBattle.Main.StartBattle();
        }
    }
}
