using UnityEngine;

public class GameFlowController
{
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
    }
    
}
