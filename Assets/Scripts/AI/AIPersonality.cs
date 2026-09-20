using UnityEngine;

[System.Serializable]
public class AIPersonality
{
    [Range(0f, 1f)]
    public float riskTaking = 0.5f;

    [Range(0f, 1f)]
    public float reactionSpeed = 0.7f;

    [Range(0f, 1f)]
    public float randomness = 0.2f;
}
