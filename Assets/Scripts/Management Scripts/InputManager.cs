using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Change name of this script and move card dealing methods to new inputmanager.
public class InputManager : MonoBehaviour
{
    private bool _hasPrevPos;
    private CardHolderScript _selectedHolder;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    void Click()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if raycast hit anything
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("ClickableObject")))
            {
                CardHolderScript currentHolder;

                if (hit.collider.CompareTag("Card"))
                {
                    currentHolder = hit.collider.gameObject.GetComponent<Card>().GetHolder();
                }
                else
                {
                    hit.collider.gameObject.TryGetComponent<CardHolderScript>(out currentHolder);
                }

                MakeCardOperations(currentHolder);
            }
            else
            {
                ClearSelected(false);
            }
        }
        else
        {
            ClearSelected(false);
        }
    }

    private void MakeCardOperations(CardHolderScript currentHolder)
    {
        if (_hasPrevPos)
        {
            // if same holder selected again
            if (currentHolder == _selectedHolder)
            {
                ClearSelected(false);
                return;
            }

            // if selectedholder's top card's type did not equals to target holder's top card's type
            if (currentHolder.GetListCount() > 0 && !_selectedHolder.GetTopCardType().Equals(currentHolder.GetTopCardType()))
            {
                ClearSelected(false);
                return;
            }

            // if target holder's cards moving, we can not select it
            if (currentHolder.AreCardsMoving)
            {
                return;
            }

            // if card holder is a card seller check if there is any selling card activity
            if(currentHolder.CompareTag("CardSeller") && currentHolder.GetComponent<CardSellerScript>().IsSellingCards)
            {
                ClearSelected(false);
                return;
            }

            // if holders removing any cards
            if(currentHolder.IsRemovingExtras)
            {
                ClearSelected(false);
                return;
            }

            StartCoroutine(CardManager.Instance.FlipCards(_selectedHolder, currentHolder));

            ClearSelected(true);
        }
        else
        {
            // if target holder's cards moving, we can not select it
            if (currentHolder.AreCardsMoving)
            {
                return;
            }

            // if card holder is a card seller check if there is any selling card activity
            if (currentHolder.CompareTag("CardSeller") && currentHolder.GetComponent<CardSellerScript>().IsSellingCards)
            {
                return;
            }

            if (currentHolder.GetListCount() > 0)
            {
                _hasPrevPos = true;
                _selectedHolder = currentHolder;

                StartCoroutine(CardManager.Instance.RiseUpCards(currentHolder));
            }
            else
            {
                // There is no card to move
            }
        }
    }

    private void ClearSelected(bool flipping)
    {
        if(_hasPrevPos && _selectedHolder && !flipping)
        {
            StartCoroutine(CardManager.Instance.DescendCards(_selectedHolder));
        }

        _hasPrevPos = false;
        _selectedHolder = null;
    }

    public void DealCards()
    {
        if(!CardManager.Instance.HasDealingCard && !CardManager.Instance.AnyCardRemover)
        {
            StartCoroutine(CardManager.Instance.DealCardCoroutine());
        }
    }
}
