using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
namespace SnakeGame
{
    /**
     * Class Responsible for moving the Snake with Input
     **/
    public class SnakeMovementController : MonoBehaviour
    {
        private InputAction turnAction;
        //public Snake TheSnake; will be Injected as well as reference to snakeCells
        private Vector3 _direction = Vector3.forward;
        private Snake _theSnake;
        private List<Transform> _snakeCells => _theSnake.SnakeCells;

        [SerializeField] private float _speed = 2f;
        
        private float dist;
        private Transform _prevSnakeCell;
        private Transform _currSnakeCell;

        public bool IsMoving => _theSnake.IsMoving;
        [Inject]
        public void Construct()
        {
            _theSnake = GetComponent<Snake>();
            turnAction = GetComponent<PlayerInput>().currentActionMap.FindAction("TurnSnake");
            turnAction.started += OnSnakeTurn;
        }
        
        void FixedUpdate()
        {
            if (!IsMoving) return;
            MoveWholeSnake();

        }
        
        public void OnSnakeTurn(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            if (inputVector == Vector2.zero) return;
            _direction = new Vector3(inputVector.x, 0, inputVector.y);
           _snakeCells[0].forward = _direction;
            
        }

        private void MoveWholeSnake()
        {
            _snakeCells[0].Translate(_snakeCells[0].forward * (_speed * Time.deltaTime), Space.World);
            //
            for (int i = 1; i < _snakeCells.Count; i++)
            {
                _currSnakeCell = _snakeCells[i];
                _prevSnakeCell = _snakeCells[i - 1];
                
                //dist = Vector3.Distance(_prevSnakeCell.position, _currSnakeCell.position);
                
                Vector3 newPos = _prevSnakeCell.position;
                newPos.y = _snakeCells[0].position.y;
                
                _currSnakeCell.position  = Vector3.Slerp(_currSnakeCell.position, newPos, Time.deltaTime);
                _currSnakeCell.rotation  = Quaternion.Slerp(_currSnakeCell.rotation, _prevSnakeCell.rotation, Time.deltaTime);
            }
        }



        
    }
}