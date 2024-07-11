using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardAnimationGroup : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float particleFlyDuration = 1f;
    [SerializeField] private Text tapToClaimText;
    [SerializeField] BABase backgroundAnimation;
    [SerializeField] RAByType rewardAnimationByType;
    [SerializeReference] AABase arranger;

    private bool isTriggered = false;
    private int working;
    private WaitUntil waitUntilTriggered;
    private List<RectTransform> actives = new();
    private List<RABase> activesRA = new();

    public bool IsComplete => working <= 0;

    public void SetDestination(RewardType type, Transform destination)
    {
        rewardAnimationByType[type].SetDestination(destination);
    }

    [Button(ButtonStyle.FoldoutButton)]
    public RewardAnimationGroup Setup()
    {
        isTriggered = false;
        working = 0;
        actives.Clear();
        activesRA.Clear();

        if (waitUntilTriggered == null)
        {
            waitUntilTriggered = new WaitUntil(() => isTriggered);
        }

        return this;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public RewardAnimationGroup Add(RewardType type, int amount, Action onReceiveReward)
    {
        if (type == RewardType.None)
        {
            Debug.LogError("Invalid type");
            return this;
        }

        working++;
        RABase reward = rewardAnimationByType[type];
        activesRA.Add(reward);
        actives.Add(reward.MyTransform);

        reward.Initialize(amount)
            .OnReceiveReward(onReceiveReward)
            .OnComplete(() =>
            {
                working--;
                reward.gameObject.SetActive(false);
            });

        return this;
    }

    [Button]
    public void Play()
    {
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            arranger.Initialize(actives);
            backgroundAnimation.gameObject.SetActive(true);
            backgroundAnimation.Initialize();
            backgroundAnimation.FadeIn(0.5f);

            yield return backgroundAnimation.WaitUntilCanArrange;

            for (int i = 0; i < actives.Count; i++)
            {
                actives[i].gameObject.SetActive(true);
            }

            arranger.Play(0.5f);

            yield return waitUntilTriggered;

            backgroundAnimation.FadeOut(0.5f);

            for (int i = 0; i < activesRA.Count; i++)
            {
                activesRA[i].Play(1f);
            }

            yield return new WaitUntil(() => IsComplete);

            backgroundAnimation.gameObject.SetActive(false);
        }
    }

    [Button]
    public void Trigger()
    {
        isTriggered = true;
    }

    #region UI Events
    public void OnClick()
    {
        Trigger();
    }
    #endregion
}
