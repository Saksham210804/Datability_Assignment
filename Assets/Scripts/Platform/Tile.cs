using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsAvailable { get; private set; } = true;

    [Header("References")]
    [SerializeField] private Renderer tileRenderer;

    [Header("Detection")]
    [SerializeField] private LayerMask characterLayer;

    [Header("Collapse")]
    [SerializeField] private float collapseDelay = 0.6f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float pressedDistance = 0.15f;

    private Vector3 startingPosition;
    private MaterialPropertyBlock propertyBlock;
    private Color platformColor = Color.white;

    private TilePoolManager tilePool;

    private void Awake()
    {
        startingPosition = transform.localPosition;
        propertyBlock = new MaterialPropertyBlock();
    }

    public void SetPool(TilePoolManager pool)
    {
        tilePool = pool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsAvailable)
            return;

        if (GameManager.Instance == null ||
            GameManager.Instance.CurrentState != GameState.Playing)
            return;

        bool isCharacter =
            (characterLayer.value & (1 << other.gameObject.layer)) != 0;

        if (isCharacter)
        {
            StartCoroutine(Collapse());
        }
    }

    public void SetMaterial(Material material)
    {
        if (material == null)
            return;

        tileRenderer.sharedMaterial = material;
        platformColor = material.color;

        SetColor(platformColor, 1f);
    }

    private IEnumerator Collapse()
    {
        IsAvailable = false;

        Vector3 startPosition = transform.localPosition;
        Vector3 pressedPosition =
            startPosition + Vector3.down * pressedDistance;

        float timer = 0f;

        while (timer < collapseDelay)
        {
            timer += Time.deltaTime;

            float t = timer / collapseDelay;

            transform.localPosition = Vector3.Lerp(
                startPosition,
                pressedPosition,
                t
            );

            SetColor(
                Color.Lerp(platformColor, Color.white, t),
                1f
            );

            yield return null;
        }

        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            SetColor(
                Color.white,
                Mathf.Lerp(1f, 0f, t)
            );

            yield return null;
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (tilePool != null)
        {
            tilePool.ReturnTile(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetTile()
    {
        StopAllCoroutines();

        IsAvailable = true;

        transform.localPosition = startingPosition;

        SetColor(platformColor, 1f);

        gameObject.SetActive(true);
    }

    private void SetColor(Color color, float alpha)
    {
        color.a = alpha;

        propertyBlock.SetColor("_BaseColor", color);

        tileRenderer.SetPropertyBlock(propertyBlock);
    }
}
