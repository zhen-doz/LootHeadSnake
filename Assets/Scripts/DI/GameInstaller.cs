using SnakeGame;
using UnityEngine;
using Zenject;


public class GameInstaller : MonoInstaller
{
    public SnakeGameManager GameManager;
    public GameObject SnakePrefab;
    public Transform StartingPoint;
    
    public BoxCollider PlacementArea;
    public Food FoodPrefab;
    public FoodSpawner FoodSpawner;
    
    public override void InstallBindings()
    {
        Snake snake = Container.InstantiatePrefabForComponent<Snake>(SnakePrefab, StartingPoint);
        Container.InstantiateComponent<SnakeMovementController>(snake.gameObject);
        
        Container
                    .Bind<Snake>()
                    .FromInstance(snake)
                    .AsSingle()
                    .NonLazy();
        
        
        
        FoodSpawner foodSpawner = Container.InstantiatePrefabForComponent<FoodSpawner>(FoodSpawner);
        foodSpawner.PlacementArea = PlacementArea;
        
        Container
                    .Bind<FoodSpawner>()
                    .FromInstance(foodSpawner)
                    .AsSingle()
                    .NonLazy();
        
       SnakeGameManager snakeGameManager = Container.InstantiatePrefabForComponent<SnakeGameManager>(GameManager);
               
        Container
            .Bind<SnakeGameManager>()
            .FromInstance(snakeGameManager)
            .AsSingle()
            .NonLazy();
        
        

    }
    
}