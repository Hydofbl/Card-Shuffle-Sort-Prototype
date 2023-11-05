using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<CardHolderScript> cardHolders = new List<CardHolderScript>();

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

        // Find and add all CardHolder object by their scripts into the cardHolders list
        cardHolders.AddRange(FindObjectsOfType<CardHolderScript>());
    }
}
