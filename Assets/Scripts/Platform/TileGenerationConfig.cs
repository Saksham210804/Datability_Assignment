using UnityEngine;

[CreateAssetMenu(
    fileName = "TileGenerationConfig",
    menuName = "Game/Tile Generation Config"
)]
public class TileGenerationConfig : ScriptableObject
{
   [Header("Platform Count")]
    [Min(1)]
    public int minPlatforms = 3;

    [Min(1)]
    public int maxPlatforms = 5;

    [Header("Vertical Distance")]
    [Min(0f)]
    public float minVerticalDistance = 7f;

    [Min(0f)]
    public float maxVerticalDistance = 10f;

    [Header("Platform Offset")]
    public float minHorizontalOffset = -5f;
    public float maxHorizontalOffset = 5f;

    [Header("Platform Size")]
    [Min(1)]
    public int minTilesPerRow = 4;

    [Min(1)]
    public int maxTilesPerRow = 7;

    [Min(1)]
    public int minRows = 3;

    [Min(1)]
    public int maxRows = 5;
}