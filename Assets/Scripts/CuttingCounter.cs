using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
        public OnProgressChangedEventArgs(float progressNormalized)
        {
            ProgressNormalized = progressNormalized;
        }
    }

    [SerializeField] private CuttingRecipeSO[] _cuttingRecipeSOArray;
    private int cuttingProgress;
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
                    //player carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWIthInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax));
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
            }
        }
        
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a KitchenObject here and it can be cut
            cuttingProgress++;

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWIthInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax));

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                //Cutting is done
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }

        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWIthInput(kitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO intputKitcheObjectSO)
    { 
       CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWIthInput(intputKitcheObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWIthInput(KitchenObjectSO inputKitcheObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in _cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitcheObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
