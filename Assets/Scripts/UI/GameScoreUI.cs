using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreValueText;
    // Start is called before the first frame update
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        _scoreValueText.text = "0";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        _scoreValueText.text = DeliveryManager.Instance.GetScore().ToString();
    }


}
