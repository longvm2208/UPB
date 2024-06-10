using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationGroup : MonoBehaviour
{
    [SerializeField] private bool isPlayOnEnable;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeReference] private List<AnimationBase> animations;

    private void OnEnable()
    {
        if (isPlayOnEnable)
        {
            Prepare();
            Play();
        }
    }

    public void Prepare()
    {
        animations.ForEach(a => a.Prepare());
    }

    public void Play(Action onComplete = null)
    {
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            canvasGroup.interactable = false;

            for (int i = 0; i < animations.Count; i++)
            {
                if (animations[i].Interval > 0f)
                {
                    yield return new WaitForSeconds(animations[i].Interval);
                }

                animations[i].Play();
            }

            yield return new WaitForSeconds(animations.Last().Duration);

            canvasGroup.interactable = true;
            onComplete?.Invoke();
        }
    }
}
