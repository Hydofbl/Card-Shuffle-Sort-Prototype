using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolderScript : MonoBehaviour
{
    public Queue<GameObject> CardQueue = new Queue<GameObject>();


    // X position changing
    private Vector3 cardPosOffset = new Vector3(0f, 0.1f, 0.1f);

    public Vector3 GetCardPos()
    {
        // if the card is first card of holder, then just add vectoral offset, else add full offset
        return CardQueue.Count > 0 ? transform.position + cardPosOffset * CardQueue.Count : transform.position + new Vector3(0f, cardPosOffset.y, 0f);
    }

    public void AddCard(GameObject card)
    {
        CardQueue.Enqueue(card);
    }

    public Transform GetTopCard()
    {
        return CardQueue.Dequeue().transform;
    }
}
