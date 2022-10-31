using UnityEngine;
using UnityEngine.UI;

//public class GameManager : MonoBehaviour
public class GameManager: Singletone<GameManager>
{
    public Text scoreText;

    public Text gameOverText;

    public int playerScore = 0;

    public void AddScore()
    {
        playerScore++;

        scoreText.text = playerScore.ToString();
    }

    public void PlayerDied()
    {
        gameOverText.enabled = true;

        Time.timeScale = 0;
    }
}
