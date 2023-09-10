using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

namespace SnakeGame
{
    /*
     * Main Snake-Player Logic
     */
    public class Snake : MonoBehaviour
    {
        
        [Header("Snake Building Blocks")]
        [SerializeField] private GameObject snakeCellPrefab;
        [SerializeField] private GameObject snakeHeadPrefab;
        [SerializeField] private Material regularCellMaterial;
        [SerializeField] private Material middleCellMaterial;  
        [SerializeField] private float cellsDistance = 1.1f;
        
        
        [Header("Snake Parts in Runtime")]
        public SnakeHead TheSnakeHead;
        private List<Transform> _snakeCells;
        public List<Transform> SnakeCells
        {
            get { return _snakeCells; }
            private set { _snakeCells = value; }
        }
        

        [Header("Snake Settings")]
        
        public float DangerZone = 0.5f;
        public int BeginSize = 0;
        public float SecondsToDissolve = 3f;
        public event Action<Food> FoodEaten;
        public event Action SnakeDied;
        public bool IsMoving = false;
        private int SnakeLength;
        private int SnakeDangerInd => (int)Mathf.Floor(SnakeLength * DangerZone);
        private Transform currentMiddleCell;
        
        private void Awake()
        {
            SetUpSnakeHead();
            SetUpSnakeBody();
        }

        private void SetUpSnakeHead()
        {
            GameObject newHead = Instantiate(snakeHeadPrefab, transform.position, transform.rotation);
            TheSnakeHead = newHead.GetComponent<SnakeHead>();
            TheSnakeHead.transform.SetParent(transform);
            TheSnakeHead.CollidedWithSnake += HandleSelfCollision;
            TheSnakeHead.CollidedWithFood += HandleFoodCollision;
            TheSnakeHead.CollidedWithWall += HandleWallCollision;
        }

        private void SetUpSnakeBody()
        {
            _snakeCells = new List<Transform> { TheSnakeHead.transform };
            SnakeLength = _snakeCells.Count;
            
            // for Loading and level progression - can start from different amount of snake segments
            for (int i = 1; i < BeginSize; i++)
            {
                Grow();
            }
        }
        
        private void Eat(Food food)
        {
            for (int i = 0; i < food.FoodScore; i++)
            {
                Grow();
            }
            FoodEaten?.Invoke(food);
            
        }

        /**
         * Set ups newly created snake cell ro be actual part of the snake
         */
        private void Grow()
        {
            GameObject newCell = CreateNewSnakeCell();
            newCell.AddComponent<Dissolvable>();
            newCell.GetComponent<Dissolvable>().SecondsToDissolve = SecondsToDissolve;
            newCell.transform.SetParent(transform);
            newCell.name = SnakeLength.ToString();
            currentMiddleCell = SnakeCells[SnakeDangerInd];
            SnakeCells.Add(newCell.transform);
            SnakeLength = SnakeCells.Count;
            
            if(SnakeLength > 2) UpdateSnakeMiddle();
                
        }
        
        /**
         * Creates new  snake cell in proper location (behind the last existing cell)
         */
        private GameObject CreateNewSnakeCell()
        {
            Transform lastCellTransform = SnakeCells[SnakeLength - 1];
            Vector3 newPos = lastCellTransform.position - (lastCellTransform.forward * cellsDistance);
            return Instantiate(snakeCellPrefab, newPos, lastCellTransform.rotation);
        }

        /**
         * Updates index. considered to be the middle one, and its visual marking
         */
        private void UpdateSnakeMiddle()
        {
            if(SnakeDangerInd != 1) currentMiddleCell.GetComponent<Renderer>().material = regularCellMaterial;
            SnakeCells[SnakeDangerInd].GetComponent<Renderer>().material = middleCellMaterial;
            currentMiddleCell = SnakeCells[SnakeDangerInd];
        }
       
        /**
         * Handles Collision with parts of Snake
         * if the collided cell was before the middle mark => the cut is too long, snake shoul die
         * otherwise => loses tail starting from the hit cell and back to the last cell
         */
        private void HandleSelfCollision(int snakeCell)
        {
            if (snakeCell <= SnakeDangerInd) Die();
            else LoseTail(snakeCell);
        }
        
        private void HandleWallCollision()
        {
            Die();
            
        }
        /**
         * Handles collision with food
         * Function allows to react to possible different foods with
         * possible different "nutrient value"
         */
        private void HandleFoodCollision(Food foodItem)
        {
            Eat(foodItem);
        }

        private void Die()
        {
            SnakeDied?.Invoke();
        }

        /**
         * Asynchronous function to loose tail
         * Waits until the cells are "dissolved" and then "cuts" them fro the snake
         */
        private async void LoseTail(int startingId)
        {
            TheSnakeHead.GetComponent<Collider>().enabled = false;
            
            int amount = SnakeLength - startingId;
            if (amount <= 0) return;
            
            var dissolveTasks = new Task[amount];
            for(int i = 0; i < amount; i++)
            {
                dissolveTasks[i] = SnakeCells[i + startingId].GetComponent<Dissolvable>().StartDissolve();
            }

            await Task.WhenAll(dissolveTasks);
            
            SnakeCells.RemoveRange(startingId, SnakeLength - startingId);
            SnakeLength = SnakeCells.Count;
            if(SnakeLength > 2) UpdateSnakeMiddle();
            
            TheSnakeHead.GetComponent<Collider>().enabled = true;
            
        }
        
        void Destroy()
        {
            TheSnakeHead.CollidedWithSnake -= HandleSelfCollision;
            TheSnakeHead.CollidedWithFood -= HandleFoodCollision;

        }
        

    }
}