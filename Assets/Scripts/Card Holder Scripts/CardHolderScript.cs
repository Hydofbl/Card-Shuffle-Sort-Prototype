using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolderScript : MonoBehaviour
{
    [Header("Card Holder")]
    public bool AreCardsMoving;
    [SerializeField] private Vector3 cardPosOffset = new Vector3(0f, 0.2f, 0f);
    [SerializeField] private int maxCardAmount = 15;
    [SerializeField] private bool isRemovingExtras;

    [Range(0f, 1f)]
    [SerializeField] private float CardRemoveWaitDuration = 0.5f;

    // Stored card queue
    protected List<Card> cardList = new List<Card>();

    private void Update()
    {
        // WIP: Extra card remover
        /*
        // Instead of checking it for every frame, card selling could turn to an action
        if (!isRemovingExtras && !AreCardsMoving && cardList.Count >= maxCardAmount)
        {
            isRemovingExtras = true;

            StartCoroutine(RemoveExtras());
        }
        */
    }

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

    // WIP
    private IEnumerator RemoveExtras()
    {
        while (cardList.Count > maxCardAmount)
        {
            Card removedCard = GetBottomCard();
            Destroy(removedCard.gameObject);


            // We need to find a way to move top cards to below



            yield return new WaitForSeconds(CardRemoveWaitDuration);
        }

        isRemovingExtras = false;
    }
}
