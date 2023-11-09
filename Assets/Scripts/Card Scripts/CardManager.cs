using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Card Flip")]
    [Range(0f, 1f)]
    [SerializeField] private float nextCardFlipDuration = 0.1f;

    [Header("Card Dealing")]
    public List<GameObject> CardPrefs;
    [SerializeField] private Transform dealStartTransform;
    [SerializeField] private Transform cardParent;
    [Range(0f, 1f)]
    [SerializeField] private float cardDealWaitDuration = 0.2f;

    public bool HasDealingCard;

    public static CardManager Instance;

    private void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public IEnumerator FlipCards(CardHolderScript selectedHolder, CardHolderScript targetHolder)
    {
        CardType flippedCardType = selectedHolder.GetTopCardType();

        selectedHolder.AreCardsMoving = true;
        targetHolder.AreCardsMoving = true;

        // Get flip rotation of cards according to the first card
        Vector3 rotation = GetTargetRotation(selectedHolder.transform.position, targetHolder.GetCardPos());

        Tween lastTween;

        do
        {
            Vector3 targetPos = targetHolder.GetCardPos();
            Vector3 selectedHolderPos = selectedHolder.GetCardPos();
            Card selectedCard = selectedHolder.GetTopCard();

            lastTween = CardAnimationManager.Instance.Flip(selectedCard.transform, selectedHolderPos, targetPos, rotation);

            targetHolder.AddCard(selectedCard);

            yield return new WaitForSeconds(nextCardFlipDuration);
        }
        while (selectedHolder.GetQueueCount() > 0 && selectedHolder.GetTopCardType().Equals(flippedCardType));

        lastTween.OnComplete(() =>
        {
            selectedHolder.AreCardsMoving = false;
            targetHolder.AreCardsMoving = false;
        });

    }

    private Vector3 GetTargetRotation(Vector3 startPos, Vector3 targetPos)
    {
        Vector2 direction2D = new Vector2(targetPos.x - startPos.x, targetPos.z - startPos.z);

        // if card's movement direction contains vertical value, then flip it vertically.
        // Else flip it horizontally. For that, I use direction2D vector.
        if (Mathf.Abs(direction2D.y) > 0)
        {
            direction2D.x = 0;
            direction2D.y = direction2D.y > 0 ? 1 : -1;
        }
        else
        {
            direction2D.y = 0;
            direction2D.x = direction2D.x > 0 ? 1 : -1;
        }

        // x-y -> x-z axises. So for vertical flip we use z(y) axis, and for horizontal flip x axis
        return new Vector3(180f * direction2D.y, 0f, -180f * direction2D.x);
    }

    public IEnumerator DealCardCoroutine()
    {
        HasDealingCard = true;

        foreach (var holder in GameManager.Instance.cardHolders)
        {
            // Check for dealed card amount

            Card card = Instantiate(CardPrefs[Random.Range(0, CardPrefs.Count)], dealStartTransform.localPosition, Quaternion.identity, cardParent.transform).GetComponent<Card>();

            CardAnimationManager.Instance.Move(card.transform, holder.GetCardPos());

            holder.AddCard(card);

            yield return new WaitForSeconds(cardDealWaitDuration);
        }

        HasDealingCard = false;
    }
}
