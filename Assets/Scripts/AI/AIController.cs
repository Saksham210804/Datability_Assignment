using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private PlatformDetector platformDetector;
    [SerializeField] private AIDecisionMaker decisionMaker;

    public AIState CurrentState { get; private set; }

    private bool isEliminated;

    private void Awake()
    {
        if (characterMovement == null)
            characterMovement = GetComponent<CharacterMovement>();

        if (platformDetector == null)
            platformDetector = GetComponent<PlatformDetector>();

        if (decisionMaker == null)
            decisionMaker = GetComponent<AIDecisionMaker>();

        CurrentState = AIState.Idle;
    }

    private void Update()
    {
        if (isEliminated)
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameState.Playing)
        {
            characterMovement.StopMoving();
            CurrentState = AIState.Idle;
            return;
        }

        HandleMovement();
        UpdateState();
    }

    private void HandleMovement()
    {
        Vector3 direction = decisionMaker.CurrentDirection;

        if (direction.sqrMagnitude < 0.01f)
        {
            characterMovement.StopMoving();
            return;
        }

        characterMovement.Move(direction);

        if (characterMovement.IsGrounded() &&
            !platformDetector.HasPlatformAhead(direction))
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
                CurrentState = AIState.Jumping;
            }
            else
            {
                CurrentState = AIState.Falling;
            }

            return;
        }

        CurrentState = AIState.Moving;
    }

    public void Eliminate()
    {
        if (isEliminated)
            return;

        isEliminated = true;
        CurrentState = AIState.Eliminated;

        characterMovement.StopMoving();

        GameManager gameManager =
            FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.CharacterEliminated(false);
        }
    }
}
