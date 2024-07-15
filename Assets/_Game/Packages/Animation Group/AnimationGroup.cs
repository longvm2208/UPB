using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationGroup : MonoBehaviour
{
    [SerializeField] bool isPlayOnEnable;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeReference] List<IElement> elements;

    bool isComplete;
    WaitForSeconds waitLastAnimation;

    public bool IsComplete => isComplete;

    void OnEnable()
    {
        if (waitLastAnimation == null)
        {
            IElement last = elements.Last();

            if (last is Interval)
            {
                Debug.LogError("Last element shouldn't be Interval");

                waitLastAnimation = new WaitForSeconds(0f);
            }
            else if (last is AnimationBase)
            {
                waitLastAnimation = new WaitForSeconds((last as AnimationBase).Duration);
            }
        }

        if (isPlayOnEnable)
        {
            Prepare();
            Play();
        }
    }

    public void Prepare()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i] is AnimationBase)
            {
                (elements[i] as AnimationBase).Prepare();
            }
        }
    }

    public void Play(Action onComplete = null)
    {
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            canvasGroup.interactable = false;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i] is Interval)
                {
                    yield return (elements[i] as Interval).Wait;
                }
                else if (elements[i] is AnimationBase)
                {
                    (elements[i] as AnimationBase).Play();
                }   
            }

            yield return waitLastAnimation;

            isComplete = true;
            canvasGroup.interactable = true;
            onComplete?.Invoke();
        }
    }
}
