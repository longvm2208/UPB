using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class RewardAnimationGroup : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float particleFlyDuration = 1f;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Text tapToClaimText;
    [Header("ARRANGE CIRCLE")]
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private Vector2 center;
    [SerializeField] private RectTransform[] rewardTransforms;
    //[SerializeField] private ParticleImage coinParticle;

    private bool isTriggered = false;
    private int working;
    private string placement;
    private WaitUntil waitUntilTriggered;
    private WaitForSeconds waitParticle;

    public bool IsComplete => working <= 0;

    [Button(ButtonStyle.FoldoutButton)]
    public RewardAnimationGroup SetupCinematic(string placement)
    {
        this.placement = placement;

        isTriggered = false;
        working = 0;
        waitUntilTriggered = new WaitUntil(() => isTriggered);
        waitParticle = new WaitForSeconds(particleFlyDuration);

        StartCoroutine(Cinematic());

        IEnumerator Cinematic()
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.ChangeAlpha(0f);
            backgroundImage.DOFade(230f / 255f, fadeDuration);
            tapToClaimText.ChangeAlpha(0f);
            tapToClaimText.DOFade(1f, fadeDuration);

            yield return waitUntilTriggered;

            backgroundImage.DOFade(0f, fadeDuration);
            tapToClaimText.DOFade(0f, fadeDuration);

            yield return new WaitUntil(() => IsComplete);

            backgroundImage.gameObject.SetActive(false);
        }

        return this;
    }

    [Button]
    public void ArrangeCircle()
    {
        List<RectTransform> actives = new List<RectTransform>();

        for (int i = 0; i < rewardTransforms.Length; i++)
        {
            if (rewardTransforms[i].gameObject.activeSelf)
            {
                actives.Add(rewardTransforms[i]);
            }
        }

        if (actives.Count == 0) return;

        if (actives.Count == 1)
        {
            actives[0].localPosition = center;
            return;
        }

        float angleModifier = 90f;

        if (actives.Count == 2)
        {
            angleModifier = 0f;
        }

        float percentage = (float)(actives.Count - 1) / (rewardTransforms.Length - 1);
        float radius = minRadius + (maxRadius - minRadius) * percentage;

        float angleStep = 360f / actives.Count;

        for (int i = 0; i < actives.Count; i++)
        {
            float angle = i * angleStep + angleModifier;
            float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            actives[i].localPosition = center + new Vector2(x, y);
        }
    }

    #region UI Events
    public void OnClick()
    {
        isTriggered = true;
    }
    #endregion
}
