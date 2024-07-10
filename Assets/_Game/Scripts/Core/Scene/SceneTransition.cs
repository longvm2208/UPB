using System;
using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool isCloseComplete = true;
    private WaitForSeconds waitClose, waitOpen;

    public bool IsCloseComplete => isCloseComplete;

    private void OnValidate()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void Initialize()
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            switch (clip.name)
            {
                case "Close":
                    waitClose = new WaitForSeconds(clip.length);
                    break;
                case "Open":
                    waitOpen = new WaitForSeconds(clip.length);
                    break;
            }
        }
    }

    public Coroutine Close(Action onComplete = null)
    {
        isCloseComplete = false;
        gameObject.SetActive(true);
        return StartCoroutine(CloseRoutine(onComplete));

        IEnumerator CloseRoutine(Action onComplete)
        {
            animator.Play("Close");
            yield return waitClose;
            isCloseComplete = true;
            onComplete?.Invoke();
        }
    }

    public Coroutine Open(Action onComplete = null)
    {
        return StartCoroutine(OpenRoutine(onComplete));

        IEnumerator OpenRoutine(Action onComplete)
        {
            animator.Play("Open");
            yield return waitOpen;
            onComplete?.Invoke();
            gameObject.SetActive(false);
        }
    }
}


