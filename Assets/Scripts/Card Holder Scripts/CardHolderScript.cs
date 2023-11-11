using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolderScript : MonoBehaviour
{
    [Header("Card Holder")]
    public bool AreCardsMoving;
    [SerializeField] private Vector3 cardPosOffset = new Vector3(0f, 0.2f, 0f);
    [SerializeField] private int maxCardAmount = 15;

    [Header("Extra Card Removing")]
    public bool IsRemovingExtras;
    [Range(0f, 1f)]
    [SerializeField] private float CardRemoveWaitDuration = 0.5f;

    // Stored card queue
    protected List<Card> cardList = new List<Card>();

    public Vector3 GetCardPos()
    {
        // For first card we just use vertical offset, so offset.y's coefficient starts with one surplus
        return transform.position + new Vector3(0f, cardPosOffset.y * (cardList.Count + 1), cardPosOffset.z * cardList.Count);
    }

    public virtual void AddCard(Card card)
    {
        cardList.Add(card);
        card.SetHolder(this);
    }

    public Card PeekTopCard()
    {
        return cardList[cardList.Count - 1];
    }

    public virtual Card GetTopCard()
    {
        Card card = cardList[cardList.Count - 1];
        cardList.Remove(card);
        return card;
    }

    public virtual Card GetBottomCard()
    {
        Card card = cardList[0];
        cardList.Remove(card);
        return card;
    }

    public CardType GetTopCardType()
    {
        return cardList[cardList.Count - 1].Type;
    }

    public List<Card> GetCardList()
    {
        return cardList;
    }

    public int GetListCount()
    {
        return cardList.Count;
    }

    public void CheckExtraCards()
    {
        // Extra card remover
        // Instead of checking it for every frame, extra card remover could turn to an action
        if (!IsRemovingExtras && !AreCardsMoving && cardList.Count >= maxCardAmount)
        {
            IsRemovingExtras = true;

            StartCoroutine(RemoveExtras());
        }
    }

    // Note: it removes from top, on original game cards removing from bottom.
    private IEnumerator RemoveExtras()
    {
        while (cardList.Count > maxCardAmount)
        {
            Card removedCard = GetTopCard();
            Destroy(removedCard.gameObject);
            yield return new WaitForSeconds(CardRemoveWaitDuration);
        }

        IsRemovingExtras = false;
    }
}
