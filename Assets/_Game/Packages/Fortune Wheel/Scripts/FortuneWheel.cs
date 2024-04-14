using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class FortuneWheel : MonoBehaviour
{
    [SerializeField]
    private RectTransform wheelTransform;
    [SerializeField, Range(1, 10)]
    private int spinCount = 5;
    [SerializeField, Range(1, 5)]
    private float spinDuration = 2f;
    [SerializeField]
    private int[] probabilities;

    [Button(ButtonStyle.FoldoutButton)]
    public void StartSpinning(Action<int> onComplete)
    {
        int index = probabilities.SelectWeightedRandomIndex();
        Vector3 rotation = (spinCount + (float)index / probabilities.Length) * 360f * Vector3.forward;
        wheelTransform.DOLocalRotate(rotation, spinDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            onComplete?.Invoke(index);
        });

        Debug.Log(index);
    }
}
