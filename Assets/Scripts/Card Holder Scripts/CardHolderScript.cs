using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolderScript : MonoBehaviour
{
    [Header("Card Holder Info")]
    public bool AreCardsMoving;

    [SerializeField] private Vector3 cardPosOffset = new Vector3(0f, 0.2f, 0f);

    // Stored card queue
    protected Stack<Card> cardQueue = new Stack<Card>();

    public Vector3 GetCardPos()
    {
        // For first card we just use vertical offset, so offset.y's coefficient starts with one surplus
        return transform.position + new Vector3(0f, cardPosOffset.y * (cardQueue.Count + 1), cardPosOffset.z * cardQueue.Count);
    }

    public virtual void AddCard(Card card)
    {
        cardQueue.Push(card);
        card.SetHolder(this);
    }

    public CardType GetTopCardType()
    {
        return cardQueue.Peek().Type;
    }

    public Card GetTopCard()
    {
        return cardQueue.Pop();
    }

    public int GetQueueCount()
    {
        return cardQueue.Count;
    }
}
