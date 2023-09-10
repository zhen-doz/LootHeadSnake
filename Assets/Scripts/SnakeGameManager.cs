using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace SnakeGame
{

   /**
    * Main Game Manager, Singlton and Observer design patterns implemented.
    * Connects between different objects events
    */
   public class SnakeGameManager : MonoBehaviour, IGameManager
   {
      //public static SnakeGameManager Instance;
      private GameFlowController _flowControl;
      private Snake _theSnake;
      
      public FoodSpawner FoodSpawner;


      public event Action GameEnded;
      public event Action<int> ScoreUpped;
      
      private bool isGameStarted = false;
      private int _score = 0;
      public int Score
      {
         get { return _score; }
         set { _score = value; }
      }
       
      [Inject]
      public void Construct(Snake snake, FoodSpawner foodSpawner)
      {
         _theSnake = snake;
         _flowControl = new GameFlowController();
         FoodSpawner = foodSpawner;
      }

      public void StartUpGame()
      {
         if(!isGameStarted)
         {
            _theSnake.transform.SetParent(null);
            _theSnake.FoodEaten += OnFoodEaten;
            _theSnake.SnakeDied += LoseGame;
            isGameStarted = true;
            FoodSpawner.IsSpawning = true;
            _theSnake.IsMoving = true;
         }

      }

      public void ContinueGame()
      {
         _flowControl.ContinueGame();
      }
      public void PauseGame()
      {
         _flowControl.PauseGame();
      }

      public void StopGame()
      {
         GameEnded?.Invoke();
      }

      public void LoseGame()
      {
         GameEnded?.Invoke();
         _theSnake.FoodEaten -= OnFoodEaten;
         _theSnake.SnakeDied -= LoseGame;
         Destroy(_theSnake);
         isGameStarted = false;
         PauseGame();
      }
      
      private void OnFoodEaten(Food food)
      {
         Score++;
         ScoreUpped?.Invoke(Score);
         food.GetEaten();
      }
   }
}

