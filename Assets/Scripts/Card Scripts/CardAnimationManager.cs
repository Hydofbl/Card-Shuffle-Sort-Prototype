using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardAnimationManager : MonoBehaviour
{
    [Header("Card Flip")]
    public float YOffset = 2f;
    [Range(0f, 1f)]
    public float FlipAnimDuration = 0.75f;

    [Range(0f, 1f)]
    public float AscendDescendAnimDuration = 0f;

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
    }

    public Tween Flip(Transform cardTransform, Vector3 startPos, Vector3 endPos, Vector3 rotation)
    {
        Vector3 midPoint = new Vector3((startPos.x + endPos.x) / 2, (startPos.y > endPos.y ? startPos.y+YOffset : endPos.y+YOffset), (startPos.z + endPos.z) / 2);

        Vector3[] pathValues = {startPos, midPoint, endPos };

        return cardTransform.DOPath(pathValues, FlipAnimDuration, PathType.CatmullRom)
            .OnStart(() => { cardTransform.DORotate(rotation, FlipAnimDuration)
                .OnComplete(() => { cardTransform.rotation = Quaternion.identity; }); });
    }

    public void Move(Transform cardTransform, Vector3 targetPos)
    {
        cardTransform.DOMove(targetPos, FlipAnimDuration);
    }

    public void Ascend(Transform cardTransform)
    {
        Vector3 cardPosition = cardTransform.position;

        cardTransform.DOMove(new Vector3(cardPosition.x, cardPosition.y + 0.5f, cardPosition.z), AscendDescendAnimDuration);
    }

    public void Descend(Transform cardTransform)
    {
        Vector3 cardPosition = cardTransform.position;

        cardTransform.DOMove(new Vector3(cardPosition.x, cardPosition.y - 0.5f, cardPosition.z), AscendDescendAnimDuration);
    }
}
