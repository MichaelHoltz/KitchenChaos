using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; private set; }
    public event EventHandler OnPickedSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter { get; private set; }
        public OnSelectedCounterChangedEventArgs(BaseCounter selectedCounter)
        {
            SelectedCounter = selectedCounter;
        }
    }
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;
    private KitchenObject _kitchenObject;
    private bool _isWalking;
    private Vector3 _lastInteractDir;
    private BaseCounter _selectedCounter;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Already an Instance");
        }
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        _gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
       if(_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return _isWalking;
    }
    private void HandleInteractions()
    {
        
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        if(moveDir != Vector3.zero)
        {
            _lastInteractDir = moveDir;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance, _countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);

                }
            }
            else
            {
                SetSelectedCounter(null);
            }
           
        }
        else
        {
            SetSelectedCounter(null);
        }

    }
    private void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float playerRadius = .7f;
        float playerHeight = 2;
        float moveDistance = _moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //cannont move towards moveDir
            //attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                //Cannot move on X, try Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z!= 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move on X or Z
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        _isWalking = moveDir != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);

    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs(_selectedCounter));
    }

    public Transform GetKichenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
        if(_kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }
    public void ClearKitchenObject()
    {
        _kitchenObject = null;

    }
    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
