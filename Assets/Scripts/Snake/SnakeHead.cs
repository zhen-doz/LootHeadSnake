using System;
using UnityEngine;
using UnityEngine.Events;

namespace SnakeGame
{
    /*
     * SnakeHead is responsible for checking for collisions and triggers
     */
    public class SnakeHead : MonoBehaviour
    {
        public event Action<int> CollidedWithSnake;
        public event Action<Food> CollidedWithFood;
        public event Action CollidedWithWall;
        
        private void OnTriggerEnter(Collider other)
        {
                if (other.gameObject.CompareTag("Food") )
                {
                    CollidedWithFood?.Invoke(other.gameObject.GetComponent<Food>());
                    return;
                }
                
                if (other.gameObject.CompareTag("Wall") )
                {
                    CollidedWithWall?.Invoke();
                    return;
                }

                if (other.gameObject.CompareTag("Snake"))
                {
                    int cellId;
                    bool isId = int.TryParse(other.gameObject.name, out cellId);
                    if(isId) CollidedWithSnake?.Invoke(cellId);
                }    
                

        }
    }
}