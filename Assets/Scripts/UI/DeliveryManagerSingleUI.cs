using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [Tooltip("The text that displays the recipe name")]
    [SerializeField] private TextMeshProUGUI _recipeNameText;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private Transform _iconTemplate;

    private void Awake()
    {
        _iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        _recipeNameText.text = recipeSO.RecipeName;

        foreach (Transform child in _iconContainer)
        {
            if (child != _iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }
        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.KitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(_iconTemplate, _iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
