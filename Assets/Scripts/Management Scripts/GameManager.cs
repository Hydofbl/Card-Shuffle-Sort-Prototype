using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<CardHolderScript> cardHolders = new List<CardHolderScript>();

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

        // Find and add all CardHolder object by their scripts into the cardHolders list. Additionally check for tags.
        cardHolders.AddRange(FindObjectsOfType<CardHolderScript>().Where(holder => holder.CompareTag("CardHolder")));
    }

    public void AddCoin(int amount)
    {
        CoinAmount += amount;
        CoinText.text = CoinAmount.ToString();
    }
}
