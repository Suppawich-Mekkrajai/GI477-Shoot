using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Button Setup")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button shootButton;

    [Header("UI Setup")] 
    [SerializeField] private TMP_Text greetingText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image crosshair;
    public static event Action OnUIStartButtonPressed;
    public static event Action OnUIRestartButtonPressed;
    public static event Action OnUIShootButtonPressed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      startButton.onClick.AddListener(OnStartButtonPressed);  
      restartButton.onClick.AddListener(OnRestartButtonPressed);
      shootButton.onClick.AddListener(OnShootButtonPressed);
      crosshair.gameObject.SetActive(false);
      scoreText.gameObject.SetActive(false);
    }

    private void OnStartButtonPressed()
    {
        OnUIStartButtonPressed?.Invoke();
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        shootButton.gameObject.SetActive(true);
        
        greetingText.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
    }
    private void OnRestartButtonPressed()
    {
        OnUIRestartButtonPressed?.Invoke();
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }
    private void OnShootButtonPressed()
    {
        OnUIShootButtonPressed?.Invoke();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"SCORE: {score}";
    }
}
