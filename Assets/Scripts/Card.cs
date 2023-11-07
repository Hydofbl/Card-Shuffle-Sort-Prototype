using System.Collections;
using System.Collections.Generic;
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
    public CardType Type;

    public CardType GetCardType()
    {
        return Type;
    }
}