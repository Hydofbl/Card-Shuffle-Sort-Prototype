using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Transform StartTransform;
    public GameObject CardPref;

    private bool _hasPrevPos;
    private Vector3 _previousPos;

    private void Start()
    {
        // unnecessary
        _previousPos = Vector3.zero;
    }

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
                if (!_hasPrevPos)
                {
                    _hasPrevPos = true;
                    _previousPos = holder.GetCardPos();
                }
                else
                {
                    Debug.Log(holder.GetCardPos());

                    CardAnimationManager.Instance.Flip(holder.GetTopCard(), _previousPos, holder.GetCardPos(), new Vector3(180f, 0f, 0f));

                    _hasPrevPos = false;
                }
            }
            else
            {
                _hasPrevPos = false;
            }
        }
    }

    public void DealCards()
    {
        foreach(var holder in GameManager.Instance.cardHolders)
        {
            GameObject card = Instantiate(CardPref, StartTransform.position, Quaternion.identity);
            holder.AddCard(card);

            CardAnimationManager.Instance.Move(card.transform, holder.GetCardPos());
        }
    }
}
