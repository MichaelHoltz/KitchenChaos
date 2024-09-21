using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no Kitchen Object on the counter
            if(player.HasKitchenObject())
            {
                //Player has a Kitchen Object
                player.GetKitchenObject().SetKitchenObjectParent(this);
                
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
        return;
    }
}
