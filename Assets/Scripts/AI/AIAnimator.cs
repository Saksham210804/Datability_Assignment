using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AIAnimator : MonoBehaviour
{
    [SerializeField] private AIController aiController;
    [SerializeField] private Animator animator;

    private static readonly int StateParam = Animator.StringToHash("State");
    private int lastState = -1;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (aiController == null)
            aiController = GetComponent<AIController>();
    }

    private void Update()
    {
        int state = GetAnimationState(aiController.CurrentState);

        if (state != lastState)
        {
            animator.SetInteger(StateParam, state);
            lastState = state;
        }
    }

    private int GetAnimationState(AIState aiState)
    {
        switch (aiState)
        {
            case AIState.Moving:
                return 1;
            case AIState.Jumping:
                return 2;
            case AIState.Falling:
            case AIState.Eliminated:
                return 3;
            default:
                return 0;
        }
    }
}
