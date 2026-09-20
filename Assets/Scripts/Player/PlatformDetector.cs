using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionDistance = 1.6f;
    [SerializeField] private Vector3 detectionSize = new Vector3(1.5f, 0.3f, 1.5f);
    [SerializeField] private float detectionHeight = 0.5f;
    [SerializeField] private float groundProbeDepth = 0.8f;
    [SerializeField] private LayerMask tileLayer;

    public bool HasPlatformAhead(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return true;

        direction.Normalize();

        Vector3 aheadPoint = transform.position +
                             direction * detectionDistance +
                             Vector3.up * detectionHeight;

        float castDistance = detectionHeight + groundProbeDepth;

        if (Physics.BoxCast(
            aheadPoint,
            detectionSize * 0.5f,
            Vector3.down,
            out RaycastHit hit,
            Quaternion.identity,
            castDistance,
            tileLayer))
        {
            Tile tile = hit.collider.GetComponentInParent<Tile>();

            if (tile != null && tile.IsAvailable)
            {
                return true;
            }
        }

        return false;
    }
}
