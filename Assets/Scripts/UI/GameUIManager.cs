using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private Text playersRemainingText;

    [Header("Start")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private Text countdownText;
    [SerializeField] private int countdownFrom = 5;

    [Header("End Screens")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

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

    private void Update()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager == null)
            return;

        if (playersRemainingText != null)
            playersRemainingText.text = "Players Remaining: " + gameManager.GetRemainingPlayers();

        if (gameManager.CurrentState == GameState.GameOver && gameOverPanel != null)
            gameOverPanel.SetActive(true);

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
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
