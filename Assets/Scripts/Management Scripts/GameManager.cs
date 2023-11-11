using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Economy")]
    public int CoinAmount;
    public TMP_Text CoinText;

    public static GameManager Instance;

    private void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void AddCoin(int amount)
    {
        CoinAmount += amount;
        CoinText.text = CoinAmount.ToString();
    }
}
