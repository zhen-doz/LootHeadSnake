namespace SnakeGame
{
    public interface IGameManager
    {
        public void ContinueGame();
        public void PauseGame();

        public void StopGame();

        public void LoseGame();
    }
}