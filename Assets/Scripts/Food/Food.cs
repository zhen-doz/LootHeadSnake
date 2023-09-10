using Zenject;
using UnityEngine;
using UnityEngine.Pool;

namespace SnakeGame
{
    /**
     * Responsoble for Food Behaviour. Allows expansion to different kinds of foods with different nutrient value
     */
     public enum FoodType
     {
         SimpleFood = 0,
     } 
     public class Food : MonoBehaviour, IFood
     {
        private int _foodScore = 0;
        public int FoodScore
        {
            get { return _foodScore; }
            private set { _foodScore = value; }
        }
        
        private IObjectPool<Food> _foodPool;
        private FoodType _foodType;

        public FoodType FoodType
        {

            get { return _foodType; }
            private set { _foodType = value; }
        }

        void Awake()
        {
            FoodType = FoodType.SimpleFood;
            FoodScore = 1;
        }
         
        public void SetPool(IObjectPool<Food> pool) => _foodPool = pool;

        public void GetEaten()
        {
            if (_foodPool != null) _foodPool.Release(this);
            else Destroy(gameObject);

        }
    }



}

