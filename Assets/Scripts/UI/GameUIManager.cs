using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TMP_Text playersRemainingText;

    [Header("Start")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private int countdownFrom = 5;

    [Header("End Screens")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    private void OnEnable()
    {
        EventManager.characterDeath += UpdateRemainingPlayer;
        EventManager.GameOver += GameOver;
        EventManager.Victory += Victory;
    }

    private void OnDisable()
    {
        EventManager.characterDeath -= UpdateRemainingPlayer;
        EventManager.GameOver -= GameOver;
        EventManager.Victory -= Victory;
    }

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (startPanel != null)
            startPanel.SetActive(true);
    }



    private void UpdateRemainingPlayer()
    {
        GameManager gameManager = GameManager.Instance;
        if (playersRemainingText != null)
            playersRemainingText.text = "Players Remaining: " + gameManager.RemainingPlayers;
    }

    private void GameOver()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager.CurrentState == GameState.GameOver && gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void Victory()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager.CurrentState == GameState.Victory && victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    public void OnStartPressed()
    {
        if (startPanel != null)
            startPanel.SetActive(false);

        StartCoroutine(CountdownThenStart());
    }

    private IEnumerator CountdownThenStart()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        for (int count = countdownFrom; count >= 1; count--)
        {
            if (countdownText != null)
                countdownText.text = count.ToString();

            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.StartGame();
        GameManager gameManager = GameManager.Instance;
        playersRemainingText.text = "Players Remaining: " + gameManager.RemainingPlayers;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
