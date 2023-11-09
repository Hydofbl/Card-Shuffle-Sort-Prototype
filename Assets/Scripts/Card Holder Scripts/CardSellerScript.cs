using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSellerScript : CardHolderScript
{
    [Header("Card Selling")]
    public int MinCardLimitToSell = 10;

    [Range(0f, 1f)]
    [SerializeField] private float CardSellWaitDuration = 0.5f;

    public bool IsSellingCards;
    public Image FillImage;

    private void Update()
    {
        // Instead of checking it for every frame, card selling could turn to an action
        if(!IsSellingCards && cardQueue.Count >= MinCardLimitToSell && !AreCardsMoving)
        {
            IsSellingCards = true;

            StartCoroutine(SellCards());
        }
    }
    public override void AddCard(Card card)
    {
        base.AddCard(card);

        FillImage.fillAmount = cardQueue.Count > MinCardLimitToSell ? 1 : (float)cardQueue.Count/MinCardLimitToSell;
    }

    private IEnumerator SellCards()
    {
        while (cardQueue.Count > 0)
        {
            Card selledCard = cardQueue.Pop();

            GameManager.Instance.AddCoin(selledCard.CardPrice);
            Destroy(selledCard.gameObject);

            FillImage.fillAmount = cardQueue.Count > MinCardLimitToSell ? 1 : (float)cardQueue.Count / MinCardLimitToSell;

            yield return new WaitForSeconds(CardSellWaitDuration);
        }

        IsSellingCards = false;
    }
}
