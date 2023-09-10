using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace  SnakeGame
{
    /*
     * Objects Spawnes food at given time keeping only permitted amount of food in game
     * Implements using Object Pool Patten
     */
    public class FoodSpawner : MonoBehaviour
    {
        public int MaxFoodItemsSimultanious = 2;
        public int FoodAppearanceRate = 3;
        private IObjectPool<Food> _foodPool;
        [SerializeField] private Food _foodPrefab;
        private float _timer;
        private int _activeCount;
        public BoxCollider PlacementArea;
        private Bounds _bounds => PlacementArea.bounds;

        private float sinceLastSpawn = 0f;
        private bool canSpawn => (_activeCount < 2);
        public bool IsSpawning = false;
        
        [Inject]
        public void Construct()
        {
            _foodPool = new ObjectPool<Food>(CreateFood, OnTakeFoodFromPool, OnReturnFoodToPool, defaultCapacity: MaxFoodItemsSimultanious);
        }
        
        void Awake()
        {
           SetUpFoodSpawner();
        }

        void Update()
        {
            if (!IsSpawning) return;
            SpawningProcess();
        }

        private void SpawningProcess()
        {
            sinceLastSpawn += Time.deltaTime;
            if (sinceLastSpawn >= FoodAppearanceRate && canSpawn)
            {
                _foodPool.Get();
                sinceLastSpawn = 0;
            }
        }

        public void SetUpFoodSpawner()
        {
            _foodPool = new ObjectPool<Food>(CreateFood, OnTakeFoodFromPool, OnReturnFoodToPool, defaultCapacity: MaxFoodItemsSimultanious);
            
        }
        
        private Food CreateFood()
        {
            var food = Instantiate<Food>(_foodPrefab, transform);
            food.SetPool(_foodPool);
            return food;

        }

        private void OnTakeFoodFromPool(Food food)
        {
            RandomFoodPlacement(food);
            food.gameObject.SetActive(true);
            _activeCount++;
        }

        private void OnReturnFoodToPool(Food food)
        {
            _activeCount--;
            food.gameObject.SetActive(false);
        }
        
        private void RandomFoodPlacement(Food food)
        {
            float x = Random.Range(_bounds.min.x, _bounds.max.x);
            float z = Random.Range(_bounds.min.z, _bounds.max.z);
            food.transform.position = new Vector3(x, 0.2f, z);

        }
    }

}