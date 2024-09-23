using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
        public OnStateChangedEventArgs(State state)
        {
            this.state = state;
        }
    }
    public enum State
    { 
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] _burningRecipeSOArray;

    private State _state;

    private float _fryingTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private float _burningTimer;
    private BurningRecipeSO _burningRecipeSO;

    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(_fryingTimer / _fryingRecipeSO.fryingTimerMax));
                    if (_fryingTimer >= _fryingRecipeSO.fryingTimerMax)
                    {
                        //Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                        _burningRecipeSO = GetBurningRecipeSOWIthInput(GetKitchenObject().GetKitchenObjectSO());
                        _state = State.Fried;
                        _burningTimer = 0f;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(_state));
                        
                    }
                    break;
                case State.Fried:
                    _burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(_burningTimer / _burningRecipeSO.burningTimerMax));
                    if (_burningTimer >= _burningRecipeSO.burningTimerMax)
                    {
                        //Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);
                        _state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(_state));
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0f));
                    }
                    break;
                case State.Burned:
                    break;

            }

        }

    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no Kitchen Object on the counter
            if (player.HasKitchenObject())
            {
                //Player has a Kitchen Object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //player carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _fryingRecipeSO = GetfryingRecipeSOWIthInput(GetKitchenObject().GetKitchenObjectSO());
                    _state = State.Frying;
                    _fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(_state));
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(_fryingTimer / _fryingRecipeSO.fryingTimerMax));
                }
            }
            else
            {
                //Player does not have a Kitchen Object
            }
        }
        else
        {
            //There is a Kitchen Object on the counter
            if (player.HasKitchenObject())
            {
                //player is carying something
            }
            else
            {
                //player is not cayring anything
                GetKitchenObject().SetKitchenObjectParent(player);
                _state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(_state));
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0f));
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        return;
    }
    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetfryingRecipeSOWIthInput(kitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO intputKitcheObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetfryingRecipeSOWIthInput(intputKitcheObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetfryingRecipeSOWIthInput(KitchenObjectSO inputKitcheObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in _fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitcheObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWIthInput(KitchenObjectSO inputKitcheObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in _burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitcheObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}

