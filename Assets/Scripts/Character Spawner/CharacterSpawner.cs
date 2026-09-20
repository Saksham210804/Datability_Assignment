using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private float spawnHeightOffset = 0.05f;
    [SerializeField] private float minSpacing = 2f;

    private IEnumerator Start()
    {
        yield return null;

        PlaceCharacters();
    }

    private void PlaceCharacters()
    {
        CharacterMovement[] characters = FindObjectsByType<CharacterMovement>(FindObjectsSortMode.None);

        List<Tile> topTiles = GetTopPlatformTiles();

        if (topTiles.Count == 0)
            return;

        Vector3 center = GetCenter(topTiles);

        topTiles.Sort((a, b) =>
            Vector3.Distance(a.transform.position, center).CompareTo(
            Vector3.Distance(b.transform.position, center)));

        List<Vector3> usedSpots = new List<Vector3>();
        int characterIndex = 0;

        foreach (Tile tile in topTiles)
        {
            if (characterIndex >= characters.Length)
                break;

            Vector3 tilePosition = tile.transform.position;

            if (IsTooClose(usedSpots, tilePosition))
                continue;

            PlaceOnTile(characters[characterIndex].transform, tile);
            usedSpots.Add(tilePosition);
            characterIndex++;
        }
    }

    private bool IsTooClose(List<Vector3> usedSpots, Vector3 position)
    {
        foreach (Vector3 spot in usedSpots)
        {
            if (Vector3.Distance(spot, position) < minSpacing)
                return true;
        }

        return false;
    }

    private void PlaceOnTile(Transform character, Tile tile)
    {
        float topY = tile.transform.position.y;

        Renderer renderer = tile.GetComponentInChildren<Renderer>();
        if (renderer != null)
            topY = renderer.bounds.max.y;

        character.position = new Vector3(
            tile.transform.position.x,
            topY + spawnHeightOffset,
            tile.transform.position.z);

        character.rotation = Quaternion.identity;
    }

    private List<Tile> GetTopPlatformTiles()
    {
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);

        float highestY = float.MinValue;

        foreach (Tile tile in allTiles)
        {
            if (tile.gameObject.activeSelf && tile.transform.position.y > highestY)
                highestY = tile.transform.position.y;
        }

        List<Tile> topTiles = new List<Tile>();

        foreach (Tile tile in allTiles)
        {
            if (tile.gameObject.activeSelf &&
                Mathf.Abs(tile.transform.position.y - highestY) < 0.5f)
            {
                topTiles.Add(tile);
            }
        }

        return topTiles;
    }

    private Vector3 GetCenter(List<Tile> tiles)
    {
        Vector3 sum = Vector3.zero;

        foreach (Tile tile in tiles)
            sum += tile.transform.position;

        return sum / tiles.Count;
    }
}
