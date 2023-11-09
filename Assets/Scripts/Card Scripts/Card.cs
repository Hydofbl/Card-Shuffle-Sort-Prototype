using System.Collections;
using UnityEngine;

public enum CardType
{
    Blue,
    Red,
    Green,
    Yellow
}

public class Card : MonoBehaviour
{
    // Default Values
    public CardType Type = CardType.Blue;
    public int CardPrice = 10;

    private CardHolderScript _holder;

    public void SetHolder(CardHolderScript holder)
    {
        _holder = holder;
    }

    public CardHolderScript GetHolder()
    {
        return _holder;
    }
}