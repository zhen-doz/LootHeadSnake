using System;
using SnakeGame;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SnakeGameUIController : MonoBehaviour
{
    private SnakeGameManager theSnakeManager;
    [SerializeField] private TimeObserver timeObserver;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button stopBtn;
    
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private SplashScreenController splashScreen;

    private bool IsPaused = false;
    
    [Inject]
    public void Construct(SnakeGameManager snakeManager)
    {
        theSnakeManager = snakeManager;
        theSnakeManager.ScoreUpped += UpdateScore;
        theSnakeManager.GameEnded += OnGameEnded;
    }

    private void Start()
    {
        splashScreen.gameObject.SetActive(true);
        playBtn.onClick.AddListener(OnPlayClicked);
        pauseBtn.onClick.AddListener(OnPauseClicked);
        stopBtn.onClick.AddListener(OnStopClicked);
    }
    
    /**
     * Game Flow control buttons : start, pause, unpause, stop game
     */
    public void OnPlayClicked()
    {
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
        if (IsPaused)
        {
            IsPaused = false;
            theSnakeManager.ContinueGame();
        }
        else theSnakeManager.StartUpGame();
        timeObserver.IsTimeStarted = true;
    }

    public void OnPauseClicked()
    {
        IsPaused = true;
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        theSnakeManager.PauseGame();
        timeObserver.IsTimeStarted = false;
    }

    public void OnStopClicked()
    {
        theSnakeManager.StopGame();
        OnGameEnded();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }


    public void OnGameEnded()
    {
        gameOverScreen.SetActive(true);
    }
    
    
    
    
}
