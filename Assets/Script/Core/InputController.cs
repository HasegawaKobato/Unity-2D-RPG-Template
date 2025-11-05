using UnityEngine;

public class InputController
{
    private static InputSystem_Actions _inputActions;
    public static InputSystem_Actions InputActions
    {
        get
        {
            if (_inputActions == null) _inputActions = new InputSystem_Actions();
            return _inputActions;
        }
    }

}
