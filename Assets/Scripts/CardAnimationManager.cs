using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardAnimationManager : MonoBehaviour
{
    // All of them must moved to card's manager
    public bool isFlipped;
    public bool isFlipping;
    public float YValue = 2f;
    public float AnimDuration = 1f;

    public static CardAnimationManager Instance;

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

        // Move them
        isFlipped = false;
        isFlipping = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isFlipping)
            {

                if (!isFlipped)
                {
                    //Flip(StartTransform.position, EndTransform.position, new Vector3(180f, 0f, 0f));

                    isFlipped = true;
                }
                else
                {
                    //(EndTransform.position, StartTransform.position, new Vector3(0, 0f, 0f));

                    isFlipped = false;
                }
            }
        }
    }

    public void Flip(Transform cardTransform, Vector3 startPos, Vector3 endPos, Vector3 rotation)
    {
        isFlipping = true;

        Vector3 midPoint = new Vector3((startPos.x + endPos.x) / 2, YValue, (startPos.z + endPos.z) / 2);

        Vector3[] pathValues = {startPos, midPoint, endPos };

        cardTransform.DOPath(pathValues, AnimDuration, PathType.CatmullRom)
            .OnStart(() => { cardTransform.DORotate(rotation, AnimDuration); })
            .OnComplete(() => {isFlipping = false; });
    }

    public void Move(Transform cardTransform, Vector3 targetPos)
    {
        cardTransform.DOMove(targetPos, AnimDuration);
    }
}
