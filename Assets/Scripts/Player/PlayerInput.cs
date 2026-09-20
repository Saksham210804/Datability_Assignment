using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    public Vector2 MovementInput { get; private set; }

    private void Update()
    {
        if (joystick != null)
        {
            MovementInput = joystick.Direction;
        }
        else
        {
            MovementInput = Vector2.zero;
        }
    }
}
