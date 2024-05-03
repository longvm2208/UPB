using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AnimationGroup : MonoBehaviour
{
    [Tooltip("0: Start of animation\n1: End of animation")]
    [SerializeField] private int state;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeReference] private List<AnimationBase> animations;

    private IEnumerator coroutine;

    public void Init()
    {
        animations.ForEach(animation => animation.Init(state));
    }

    public void Forward(Action onComplete = null)
    {
        coroutine.Stop(this);
        coroutine = Routine();
        coroutine.Start(this);

        IEnumerator Routine()
        {
            canvasGroup.interactable = false;

            for (int i = 0; i < animations.Count; i++)
            {
                yield return new WaitForSeconds(animations[i].Interval);

                animations[i].Forward();
            }

            yield return new WaitForSeconds(animations.Last().Duration);

            canvasGroup.interactable = true;
            onComplete?.Invoke();
        }
    }

    public void Backward(Action onComplete = null)
    {
        coroutine.Stop(this);
        coroutine = Routine();
        coroutine.Start(this);

        IEnumerator Routine()
        {
            canvasGroup.interactable = false;

            for (int i = animations.Count - 1; i >= 0; i--)
            {
                animations[i].Backward();

                yield return new WaitForSeconds(animations[i].Interval);
            }

            yield return new WaitForSeconds(animations[0].Duration);

            canvasGroup.interactable = true;
            onComplete?.Invoke();
        }
    }
}
