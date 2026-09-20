using UnityEngine;

public class CharacterFallDetector : MonoBehaviour
{
    [SerializeField] private float fallBuffer = 10f;

    private bool hasFallen;

    private void Update()
    {
        if (hasFallen)
            return;

        GameManager gameManager = GameManager.Instance;

        if (gameManager == null)
            return;

        if (transform.position.y <= gameManager.GetFallHeight())
        {
            hasFallen = true;

            PlayerController player =
                GetComponent<PlayerController>();

            if (player != null)
            {
                player.Eliminate();
                return;
            }

            AIController ai =
                GetComponent<AIController>();

            if (ai != null)
            {
                ai.Eliminate();
            }
        }
    }
}
