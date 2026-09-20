using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    private static readonly int StateParam = Animator.StringToHash("State");
    private int lastState = -1;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (playerController == null)
            playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        int state = GetAnimationState(playerController.CurrentState);

        if (state != lastState)
        {
            animator.SetInteger(StateParam, state);
            lastState = state;
        }
    }

    private int GetAnimationState(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Walk:
                return 1;
            case PlayerState.Jump:
                return 2;
            case PlayerState.Fall:
            case PlayerState.Eliminated:
                return 3;
            default:
                return 0;
        }
    }
}
