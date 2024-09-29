using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipesDeliveredText;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChagned;
        Hide();

    }

    private void GameManager_OnStateChagned(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            _recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            Show();
            
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
