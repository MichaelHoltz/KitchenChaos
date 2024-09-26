using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
        public OnIngredientAddedEventArgs(KitchenObjectSO kitchenObjectSO)
        {
            this.kitchenObjectSO = kitchenObjectSO;
        }
    }

    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList = new List<KitchenObjectSO>();

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!_validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //Not a valid ingredient type
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //Already contains the ingredient type
            return false;
            
        }
        else 
        { 
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs(kitchenObjectSO));
            return true;
        }
    }

}
