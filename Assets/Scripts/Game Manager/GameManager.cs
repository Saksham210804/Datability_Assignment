using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    public int RemainingPlayers { get; private set; }

    [SerializeField] private float fallBuffer = 10f;

    private float fallHeight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CurrentState = GameState.Waiting;
    }

    public void SetLowestPlatformHeight(float lowestPlatformHeight)
    {
        fallHeight = lowestPlatformHeight - fallBuffer;
    }

    public float GetFallHeight()
    {
        return fallHeight;
    }

    public void StartGame()
    {
        RemainingPlayers = CountCharacters();

        CurrentState = GameState.Playing;
    }

    public void CharacterEliminated(bool isHuman)
    {
        if (CurrentState != GameState.Playing)
            return;

        RemainingPlayers--;

        if (isHuman)
        {
            EndMatch(GameState.GameOver);
            return;
        }

        if (RemainingPlayers <= 1)
        {
            EndMatch(GameState.Victory);
        }
    }

    public int GetRemainingPlayers()
    {
        return RemainingPlayers;
    }

    private int CountCharacters()
    {
        int players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).Length;
        int bots = FindObjectsByType<AIController>(FindObjectsSortMode.None).Length;

        return players + bots;
    }

    private void EndMatch(GameState result)
    {
        CurrentState = result;

        FreezeAllCharacters();
    }

    private void FreezeAllCharacters()
    {
        foreach (PlayerController player in FindObjectsByType<PlayerController>(FindObjectsSortMode.None))
            player.enabled = false;

        foreach (AIController bot in FindObjectsByType<AIController>(FindObjectsSortMode.None))
            bot.enabled = false;

        foreach (CharacterMovement movement in FindObjectsByType<CharacterMovement>(FindObjectsSortMode.None))
            movement.Freeze();
    }
}
