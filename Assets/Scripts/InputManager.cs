using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Change name of this script and move card dealing methods to new inputmanager.
public class InputManager : MonoBehaviour
{
    [Header("Card Dealing")]
    public GameObject CardPref;
    public Transform StartTransform;
    public Transform CardParent;
    [Range(0f, 1f)]
    public float cardDealWaitDuration = 0.2f;

    private bool _hasPrevPos;
    private CardHolderScript _selectedHolder;

    void Update()
    {
        //check if the left mouse has been pressed down this frame
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    void Click()
    {
        //empty RaycastHit object which raycast puts the hit details into
        RaycastHit hit;

        //ray shooting out of the camera from where the mouse is
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.TryGetComponent<CardHolderScript>(out CardHolderScript holder))
            {
                if (_hasPrevPos)
                {
                    // if same holder selected again
                    if(holder == _selectedHolder)
                    {
                        ClearSelected();
                        return;
                    }

                    FlipCard(holder);

                    ClearSelected();
                }
                else
                {
                    if (holder.GetQueueCount() > 0)
                    {
                        _hasPrevPos = true;
                        _selectedHolder = holder;
                    }
                    else
                    {
                        // There is no card to move
                    }
                }
            }
            else
            {
                ClearSelected();
            }
        }
        else
        {
            ClearSelected();
        }
    }

    public void DealCards()
    {
        StartCoroutine(DealCardCoroutine());
    }

    public IEnumerator DealCardCoroutine()
    {
        foreach (var holder in GameManager.Instance.cardHolders)
        {
            // Check for dealed card amounth

            Card card = Instantiate(CardPref, StartTransform.localPosition, Quaternion.identity, CardParent.transform).GetComponent<Card>();

            CardAnimationManager.Instance.Move(card.transform, holder.GetCardPos());

            holder.AddCard(card);

            yield return new WaitForSeconds(cardDealWaitDuration);
        }
    }

    private void ClearSelected()
    {
        _hasPrevPos = false;
        _selectedHolder = null;
    }

    private void FlipCard(CardHolderScript holder)
    {
        Vector3 targetPos = holder.GetCardPos();
        Vector3 rotation = new Vector3(180f, 0f, 0f);
        Vector3 selectedHolderPos = _selectedHolder.GetCardPos();
        Card selectedCard = _selectedHolder.GetTopCard();

        CardAnimationManager.Instance.Flip(selectedCard.transform, selectedHolderPos, targetPos, rotation);

        holder.AddCard(selectedCard);
    }
}
