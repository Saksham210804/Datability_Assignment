
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float fallBuffer = 10f;
    [SerializeField] private int totalPlayers = 5;
    private float fallHeight;
    private GameState currentState;
    private int remainingPlayers;
    private CharacterMovement[] characterMovements;
    public static GameManager Instance { get; private set; }

    public GameState CurrentState
    { get { return currentState; }
      private set { currentState = value; }
    }

    public int RemainingPlayers
    { get { return remainingPlayers; } 
      private set { remainingPlayers = value; }
    }

    public float FallHeight
    {
        get { return fallHeight;}
        private set { fallHeight = value; }
    }

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
        FallHeight = lowestPlatformHeight - fallBuffer;
    }

    public void StartGame()
    {
        RemainingPlayers = totalPlayers;

        CurrentState = GameState.Playing;
        characterMovements = FindObjectsByType<CharacterMovement>(FindObjectsSortMode.None);

        ActivateAllCharacters();
    }

    private void ActivateAllCharacters()
    {
        foreach (CharacterMovement movement in characterMovements)
            movement.Activate();
    }

    public void CharacterEliminated(bool isHuman)
    {
        if (CurrentState != GameState.Playing)
            return;

        RemainingPlayers--;
        EventManager.characterDeath?.Invoke();

        if (isHuman)
        {
            PlayerLoose();
            return;
        }

        if (RemainingPlayers <= 1)
        {
            PlayerWin();
        }
    }


    private void PlayerLoose()
    {
        CurrentState = GameState.GameOver;
        EventManager.GameOver?.Invoke();
        FreezeAllCharacters();
    }

    private void PlayerWin()
    {
        CurrentState = GameState.Victory;
        EventManager.Victory?.Invoke();
        FreezeAllCharacters();
    }

   

    private void FreezeAllCharacters()
    {

        foreach (CharacterMovement movement in characterMovements)
            movement.Freeze();
    }
}
