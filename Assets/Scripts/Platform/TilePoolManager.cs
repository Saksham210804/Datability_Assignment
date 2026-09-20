using System.Collections.Generic;
using UnityEngine;

public class TilePoolManager : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private int startingPoolSize = 980;

    private Queue<Tile> availableTiles = new Queue<Tile>();

    private void Awake()
    {
        for (int i = 0; i < startingPoolSize; i++)
        {
            CreateTile();
        }
    }

    private void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, transform);

        tile.SetPool(this);

        tile.gameObject.SetActive(false);

        availableTiles.Enqueue(tile);
    }

    public Tile GetTile()
    {
        if (availableTiles.Count == 0)
        {
            CreateTile();
        }

        Tile tile = availableTiles.Dequeue();

        tile.ResetTile();

        return tile;
    }

    public void ReturnTile(Tile tile)
    {
        if (tile == null)
            return;

        tile.ResetTile();
        tile.gameObject.SetActive(false);

        availableTiles.Enqueue(tile);
    }
}
