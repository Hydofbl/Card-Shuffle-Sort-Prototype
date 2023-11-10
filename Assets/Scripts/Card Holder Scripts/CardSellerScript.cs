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
    [SerializeField] private Image FillImage;
    [SerializeField] private ParticleSystem cardSellEffect;

    public bool IsSellingCards;

    private void Update()
    {
        // Instead of checking it for every frame, card selling could turn to an action
        if (!IsSellingCards && cardList.Count >= MinCardLimitToSell && !AreCardsMoving)
        {
            IsSellingCards = true;

            StartCoroutine(SellCards());
        }
    }

    public override void AddCard(Card card)
    {
        base.AddCard(card);

        FillCardAmountIndicator();
    }

    public override Card GetTopCard()
    {
        Card card = cardList[cardList.Count - 1];
        cardList.Remove(card);

        FillCardAmountIndicator();
        return card;
    }

    private IEnumerator SellCards()
    {
        while (cardList.Count > 0)
        {
            Card soldCard = cardList[cardList.Count - 1];
            cardList.Remove(soldCard);

            GameManager.Instance.AddCoin(soldCard.CardPrice);

            PlaySellEffect(soldCard);
            Destroy(soldCard.gameObject);

            FillCardAmountIndicator();

            yield return new WaitForSeconds(CardSellWaitDuration);
        }

        IsSellingCards = false;
    }

    private void FillCardAmountIndicator()
    {
        FillImage.fillAmount = cardList.Count > MinCardLimitToSell ? 1 : (float)cardList.Count / MinCardLimitToSell;
    }

    private void PlaySellEffect(Card soldCard)
    {
        var psMain = cardSellEffect.main;
        psMain.startColor = soldCard.GetComponent<Renderer>().material.color;
        Destroy(Instantiate(cardSellEffect, soldCard.transform.position, Quaternion.identity), psMain.startLifetime.constantMax);
    }
}
