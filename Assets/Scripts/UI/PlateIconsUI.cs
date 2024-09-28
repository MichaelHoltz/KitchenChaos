using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlateIconsUI : MonoBehaviour
{
    [Tooltip("The PlateKitchenObject that this UI represents")]
    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private Transform _iconTemplate;

    private void Awake()
    {
        _iconTemplate.gameObject.SetActive(false);
    }


    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child != _iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KitchenObjectSO kitchenObjectSO in _plateKitchenObject.GetKitchenObjectSOList())
        {
            //Update the visual representation of the plate
            Transform iconTransform = Instantiate(_iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
