using System;

using TMPro;

using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChagned;
        Hide();

    }

    private void GameManager_OnStateChagned(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        _countdownText.text = Math.Ceiling( GameManager.Instance.GetCountdownToStartTimer()).ToString("0");
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
