using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolderScript : MonoBehaviour
{
    // Stored card queue
    private Stack<Card> CardQueue = new Stack<Card>();

    private Vector3 cardPosOffset = new Vector3(0f, 0.1f, 0.1f);

    public Vector3 GetCardPos()
    {
        // For first card we just use vertical offset, so offset.y's coefficient starts with one surplus
        return transform.position + new Vector3(0f, cardPosOffset.y * (CardQueue.Count + 1), cardPosOffset.z * CardQueue.Count);
    }

    public void AddCard(Card card)
    {
        CardQueue.Push(card);
    }

    public Card GetTopCard()
    {
        return CardQueue.Pop();
    }

    public int GetQueueCount()
    {
        return CardQueue.Count;
    }
}
