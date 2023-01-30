using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivePlayerManager : MonoBehaviour
{
    [SerializeField] public ActivePlayer player1;
    [SerializeField] public ActivePlayer player2;
    [SerializeField] private float maxTimePerTurn;
    [SerializeField] private float timeBetweenTurns;

    public ActivePlayer currentPlayer;
    private float currentTurnTime;
    private float currentDelay;
    private float timerNum;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI playerWinText;

    [SerializeField] public PlayerHealth playerHealth1;
    [SerializeField] public PlayerHealth playerHealth2;
    public PlayerHealth currentHealthCheck;

    public bool gameEnded = false;

    void Start()
    {
        player1.AssignManager(this);
        player2.AssignManager(this);

        currentPlayer = player1;
        currentHealthCheck = playerHealth2; 
    }

    private void Update()
    {        
        if(currentDelay <= 0 && !gameEnded)
        {
            currentTurnTime += Time.deltaTime;
            timerNum = Mathf.Round(currentTurnTime * 100f) / 100f;
            timerText.SetText(timerNum.ToString());

            CheckHealth();

            if (currentTurnTime >= maxTimePerTurn)
            {
                ChangeTurn();
                ResetTimers();
            }
        }
        else
        {
            currentDelay -= Time.deltaTime;
            timerText.SetText("0:00");
        }
    }

    public bool PlayerCanPlay()
    {
        return currentDelay <= 0;
    }

    public ActivePlayer GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void ChangeTurn()
    {
        if (player1 == currentPlayer)
        {
            currentPlayer = player2;
            currentHealthCheck = playerHealth1;
        }
        else if (player2 == currentPlayer)
        {
            currentPlayer = player1;
            currentHealthCheck = playerHealth2;
        }

        ResetTimers();
    }

    private void ResetTimers()
    {
        currentTurnTime = 0;
        currentDelay = timeBetweenTurns;
    }

    private void CheckHealth()
    {
        if (currentHealthCheck.currentHealth <= 0)
        {
            winScreen.SetActive(true);
            timerUI.SetActive(false);
            gameEnded = true;

            if (currentHealthCheck == playerHealth1)
            {
                playerWinText.SetText("Player 2");
            }

            if (currentHealthCheck == playerHealth2)
            {
                playerWinText.SetText("Player 1");
            }
        }
    }
}
