using UnityEngine;

public class AIDecisionMaker : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float searchRadius = 8f;
    [SerializeField] private LayerMask tileLayer;

    [Header("Decision")]
    [SerializeField] private float decisionInterval = 0.35f;

    [SerializeField] private AIPersonality personality;

    private float decisionTimer;
    private Vector3 currentDirection;

    public Vector3 CurrentDirection => currentDirection;

    private void Update()
    {
        decisionTimer -= Time.deltaTime;

        if (decisionTimer <= 0f)
        {
            MakeDecision();

            float reactionDelay = Mathf.Lerp(
                0.15f,
                0.5f,
                1f - personality.reactionSpeed
            );

            decisionTimer = decisionInterval + reactionDelay;
        }
    }

    private void MakeDecision()
    {
        Collider[] nearbyTiles = Physics.OverlapSphere(
            transform.position,
            searchRadius,
            tileLayer
        );

        Vector3 bestDirection = Vector3.zero;
        float bestScore = float.MinValue;

        foreach (Collider collider in nearbyTiles)
        {
            Tile tile = collider.GetComponentInParent<Tile>();

            if (tile == null || !tile.IsAvailable)
                continue;

            Vector3 directionToTile =
                tile.transform.position - transform.position;

            directionToTile.y = 0f;

            float distance = directionToTile.magnitude;

            if (distance < 0.5f)
                continue;

            directionToTile.Normalize();

            float forwardScore = Vector3.Dot(
                transform.forward,
                directionToTile
            );

            if (forwardScore < -0.2f)
                continue;

            float distanceScore =
                1f - Mathf.Clamp01(distance / searchRadius);

            float score =
                forwardScore * 2f +
                distanceScore;

            score += Random.Range(
                0f,
                personality.randomness
            );

            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = directionToTile;
            }
        }

        if (bestDirection.sqrMagnitude > 0.01f)
        {
            currentDirection = bestDirection;
        }
        else
        {
            currentDirection = GetRandomDirection();
        }
    }

    private Vector3 GetRandomDirection()
    {
        Vector2 random = Random.insideUnitCircle.normalized;

        return new Vector3(
            random.x,
            0f,
            random.y
        );
    }
}
