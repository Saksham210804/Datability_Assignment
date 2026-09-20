using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float touchRadius = 150f;

    public Vector2 MovementInput { get; private set; }

    private int activeFingerId = -1;
    private Vector2 touchStartPosition;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            MovementInput = Vector2.zero;
            activeFingerId = -1;
            return;
        }

        if (activeFingerId == -1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                activeFingerId = touch.fingerId;
                touchStartPosition = touch.position;
                MovementInput = Vector2.zero;
            }

            return;
        }

        Touch activeTouch = GetActiveTouch();

        if (activeTouch.fingerId == -1)
        {
            MovementInput = Vector2.zero;
            activeFingerId = -1;
            return;
        }

        if (activeTouch.phase == TouchPhase.Ended ||
            activeTouch.phase == TouchPhase.Canceled)
        {
            MovementInput = Vector2.zero;
            activeFingerId = -1;
            return;
        }

        Vector2 touchOffset = activeTouch.position - touchStartPosition;

        MovementInput = Vector2.ClampMagnitude(
            touchOffset / touchRadius,
            1f
        );
    }

    private Touch GetActiveTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.fingerId == activeFingerId)
            {
                return touch;
            }
        }

        return default;
    }
}
