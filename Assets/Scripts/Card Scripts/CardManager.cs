using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Card Holder")]
    public List<CardHolderScript> CardHolders = new List<CardHolderScript>();

    [Header("Card Flip")]
    [Range(0f, 1f)]
    [SerializeField] private float nextCardFlipDuration = 0.1f;

    [Header("Card Dealing")]
    public bool HasDealingCard;
    public List<GameObject> CardPrefs;
    [SerializeField] private Transform dealStartTransform;
    [SerializeField] private Transform cardParent;
    [SerializeField] private int dealCardAmount = 4;
    [Range(0f, 1f)]
    [SerializeField] private float cardDealWaitDuration = 0.2f;

    [Header("Card Ascend/Descend")]
    [Range(0f, 1f)]
    [SerializeField] private float ascendDescendDuration = 0f;

    public bool AnyCardRemover
    {
        get { return CardHolders.Any(holder => holder.IsRemovingExtras); }
    }

    public static CardManager Instance;

    private void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }

        // Find and add all CardHolder object by their scripts into the cardHolders list. Additionally check for tags.
        CardHolders.AddRange(FindObjectsOfType<CardHolderScript>().Where(holder => holder.CompareTag("CardHolder")));

        StartCoroutine(DealCardCoroutine());
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

            selectedCard.GetComponent<BoxCollider>().enabled = false;
            lastTween = CardAnimationManager.Instance.Flip(selectedCard.transform, selectedHolderPos, targetPos, rotation);

            targetHolder.AddCard(selectedCard);

            yield return new WaitForSeconds(nextCardFlipDuration);
        }
        while (selectedHolder.GetListCount() > 0 && selectedHolder.GetTopCardType().Equals(flippedCardType));

        if(targetHolder.CompareTag("CardSeller"))
        {
            selectedHolder.AreCardsMoving = false;

            // wait last tween to complete before selling cards
            lastTween.OnComplete(() =>
            {
                targetHolder.AreCardsMoving = false;
            });
        }
        else
        {
            selectedHolder.AreCardsMoving = false;
            targetHolder.AreCardsMoving = false;
        }
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

        Tween lastTween = null;

        CardHolders.ForEach(holder => { holder.AreCardsMoving = true; });

        foreach (var holder in CardHolders)
        {
            Vector3 rotation = GetTargetRotation(dealStartTransform.position, holder.GetCardPos());

            for (int i = 0; i < dealCardAmount; i++)
            {
                Card card = Instantiate(CardPrefs[Random.Range(0, CardPrefs.Count)], dealStartTransform.localPosition, Quaternion.identity, cardParent.transform).GetComponent<Card>();
                
                Vector3 targetPos = holder.GetCardPos();

                lastTween = CardAnimationManager.Instance.Flip(card.transform, dealStartTransform.position, targetPos, rotation);

                holder.AddCard(card);

                yield return new WaitForSeconds(cardDealWaitDuration);
            }
        }

        lastTween.OnComplete(() => {
            DOTween.KillAll();
            CardHolders.ForEach(holder => { 
                holder.AreCardsMoving = false; 
                holder.CheckExtraCards(); 
                HasDealingCard = false; 
            }); 
        });
    }

    public IEnumerator RiseUpCards(CardHolderScript selectedHolder)
    {
        // Sometimes selectedHolder becomes null because of other parallel processes.
        // So we copy it.
        CardHolderScript cardHolder = selectedHolder;

        CardType RiseUppedCardType = cardHolder.GetTopCardType();
        List<Card> cards = cardHolder.GetCardList();

        for (int i = cards.Count - 1; i >= 0; i--)
        {
            if (!cards[i].Type.Equals(RiseUppedCardType))
            {
                break;
            }

            CardAnimationManager.Instance.Ascend(cards[i].transform);

            yield return new WaitForSeconds(ascendDescendDuration);
        }
    }

    public IEnumerator DescendCards(CardHolderScript selectedHolder)
    {
        // Sometimes selectedHolder becomes null because of other parallel processes.
        // So we copy it.
        CardHolderScript cardHolder = selectedHolder;

        CardType RiseUppedCardType = cardHolder.GetTopCardType();
        List<Card> cards = cardHolder.GetCardList();

        for (int i = cards.Count - 1; i >= 0; i--)
        {
            if (!cards[i].Type.Equals(RiseUppedCardType))
            {
                break;
            }

            CardAnimationManager.Instance.Descend(cards[i].transform);

            yield return new WaitForSeconds(ascendDescendDuration);
        }
    }
}
