using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private PlatformDetector platformDetector;

    public PlayerState CurrentState { get; private set; }

    private bool isEliminated;

    private void Awake()
    {
        CurrentState = PlayerState.Idle;

        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (characterMovement == null)
            characterMovement = GetComponent<CharacterMovement>();

        if (platformDetector == null)
            platformDetector = GetComponent<PlatformDetector>();
    }

    private void Update()
    {
        if (isEliminated)
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameState.Playing)
        {
            characterMovement.StopMoving();
            CurrentState = PlayerState.Idle;
            return;
        }

        HandleMovement();
        UpdateState();
    }

    private void HandleMovement()
    {
        Vector2 input = playerInput.MovementInput;

        Vector3 movementDirection = new Vector3(
            input.x,
            0f,
            input.y
        );

        if (movementDirection.sqrMagnitude < 0.01f)
        {
            characterMovement.StopMoving();
            return;
        }

        characterMovement.Move(movementDirection);

        if (characterMovement.IsGrounded() &&
            !platformDetector.HasPlatformAhead(movementDirection))
        {
            characterMovement.Jump();
        }
    }

    private void UpdateState()
    {
        if (!characterMovement.IsGrounded())
        {
            if (characterMovement.IsMovingUp())
            {
                CurrentState = PlayerState.Jump;
            }
            else
            {
                CurrentState = PlayerState.Fall;
            }

            return;
        }

        if (playerInput.MovementInput.sqrMagnitude > 0.01f)
        {
            CurrentState = PlayerState.Walk;
        }
        else
        {
            CurrentState = PlayerState.Idle;
        }
    }

    public void Eliminate()
    {
        if (isEliminated)
            return;

        isEliminated = true;
        CurrentState = PlayerState.Eliminated;

        characterMovement.StopMoving();

        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.CharacterEliminated(true);
        }
    }
}
