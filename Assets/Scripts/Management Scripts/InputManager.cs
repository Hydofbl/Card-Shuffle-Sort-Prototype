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
                ClearSelected();
            }
        }
        else
        {
            ClearSelected();
        }
    }

    private void MakeCardOperations(CardHolderScript currentHolder)
    {
        if (_hasPrevPos)
        {
            // if same holder selected again
            if (currentHolder == _selectedHolder)
            {
                ClearSelected();
                return;
            }

            // if selectedholder's top card's type did not equals to target holder's top card's type
            if (currentHolder.GetQueueCount() > 0 && !_selectedHolder.GetTopCardType().Equals(currentHolder.GetTopCardType()))
            {
                ClearSelected();
                return;
            }

            // if target holder's cards moving, we can not select it
            if (currentHolder.AreCardsMoving)
            {
                ClearSelected();
                return;
            }

            if(currentHolder.CompareTag("CardSeller") && currentHolder.GetComponent<CardSellerScript>().IsSellingCards)
            {
                ClearSelected();
                return;
            }

            StartCoroutine(CardManager.Instance.FlipCards(_selectedHolder, currentHolder));

            ClearSelected();
        }
        else
        {
            if (currentHolder.AreCardsMoving)
            {
                ClearSelected();
                return;
            }

            if (currentHolder.CompareTag("CardSeller") && currentHolder.GetComponent<CardSellerScript>().IsSellingCards)
            {
                ClearSelected();
                return;
            }

            if (currentHolder.GetQueueCount() > 0)
            {
                _hasPrevPos = true;
                _selectedHolder = currentHolder;
            }
            else
            {
                // There is no card to move
            }
        }
    }

    private void ClearSelected()
    {
        _hasPrevPos = false;
        _selectedHolder = null;
    }

    public void DealCards()
    {
        if(!CardManager.Instance.HasDealingCard)
        {
            StartCoroutine(CardManager.Instance.DealCardCoroutine());
        }
    }
}
