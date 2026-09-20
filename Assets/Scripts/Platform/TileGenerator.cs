using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TilePoolManager tilePool;
    [SerializeField] private TileGenerationConfig generationConfig;

    [Header("Platform Materials")]
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material purpleMaterial;
    [SerializeField] private Material yellowMaterial;

    private Material[] platformMaterials;

    private float tileWidth;
    private float tileDepth;

    private float lowestPlatformHeight;

    private void Awake()
    {
        platformMaterials = new Material[]
        {
            redMaterial,
            blueMaterial,
            greenMaterial,
            purpleMaterial,
            yellowMaterial
        };
    }

    private void Start()
    {
        CalculateTileDimensions();
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        if (tilePool == null || generationConfig == null)
        {
            Debug.LogError("TileGenerator is missing a reference.");
            return;
        }

        int platformCount = Random.Range(
            generationConfig.minPlatforms,
            generationConfig.maxPlatforms + 1
        );

        Vector3 platformPosition = transform.position;

        lowestPlatformHeight = platformPosition.y;

        for (int i = 0; i < platformCount; i++)
        {
            GeneratePlatform(platformPosition);

            if (platformPosition.y < lowestPlatformHeight)
            {
                lowestPlatformHeight = platformPosition.y;
            }

            if (i < platformCount - 1)
            {
                platformPosition = GetNextPlatformPosition(platformPosition);
            }
        }

        GameManager gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            gameManager.SetLowestPlatformHeight(lowestPlatformHeight);
        }
    }

    private void GeneratePlatform(Vector3 center)
    {
        int rows = Random.Range(
            generationConfig.minRows,
            generationConfig.maxRows + 1
        );

        int tilesPerRow = Random.Range(
            generationConfig.minTilesPerRow,
            generationConfig.maxTilesPerRow + 1
        );

        Material platformMaterial = GetRandomMaterial();

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < tilesPerRow; column++)
            {
                Vector3 tilePosition = GetTilePosition(
                    center,
                    row,
                    column,
                    rows,
                    tilesPerRow
                );

                Tile tile = tilePool.GetTile();

                if (tile == null)
                    return;

                tile.transform.position = tilePosition;
                tile.SetMaterial(platformMaterial);
            }
        }
    }

    private Vector3 GetTilePosition(
        Vector3 center,
        int row,
        int column,
        int rows,
        int columns)
    {
        float horizontalSpacing = tileWidth * 0.75f;
        float verticalSpacing = tileDepth;

        float totalWidth =
            (columns - 1) * horizontalSpacing;

        float x =
            column * horizontalSpacing -
            totalWidth * 0.5f;

        float totalDepth =
            (rows - 1) * verticalSpacing;

        float z =
            row * verticalSpacing -
            totalDepth * 0.5f;

        if (row % 2 != 0)
        {
            x += horizontalSpacing * 0.5f;
        }

        return center + new Vector3(
            x,
            0f,
            z
        );
    }

    private Vector3 GetNextPlatformPosition(Vector3 currentPosition)
    {
        float verticalDistance = Random.Range(
            generationConfig.minVerticalDistance,
            generationConfig.maxVerticalDistance
        );

        float xOffset = Random.Range(
            generationConfig.minHorizontalOffset,
            generationConfig.maxHorizontalOffset
        );

        float zOffset = Random.Range(
            generationConfig.minHorizontalOffset,
            generationConfig.maxHorizontalOffset
        );

        return currentPosition + new Vector3(
            xOffset,
            -verticalDistance,
            zOffset
        );
    }

    private Material GetRandomMaterial()
    {
        int index = Random.Range(
            0,
            platformMaterials.Length
        );

        return platformMaterials[index];
    }

    private void CalculateTileDimensions()
    {
        Tile tile = tilePool.GetTile();

        if (tile == null)
        {
            Debug.LogError("Unable to get a Tile from the pool.");
            return;
        }

        Renderer renderer = tile.GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogError("Tile does not have a Renderer.");
            tilePool.ReturnTile(tile);
            return;
        }

        tileWidth = renderer.bounds.size.x;
        tileDepth = renderer.bounds.size.z;

        tilePool.ReturnTile(tile);

        Debug.Log(
            "Tile dimensions: " +
            tileWidth + " x " +
            tileDepth
        );
    }
}