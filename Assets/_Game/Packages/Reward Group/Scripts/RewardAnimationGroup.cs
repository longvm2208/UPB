//using AssetKits.ParticleImage;
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
    [Header("COIN")]
    [SerializeField] private GameObject coin;
    [SerializeField] private RectTransform coinIconTransform;
    [SerializeField] private TMP_Text coinAmountTmp;
    //[SerializeField] private ParticleImage coinParticle;

    [Header("INFINITE LIFE")]
    [SerializeField] private GameObject infiniteLife;
    [SerializeField] private RectTransform infiniteLifeIconTransform;
    [SerializeField] private TMP_Text infiniteLifeDurationTmp;
    [SerializeField] private RectTransform infiniteLifeDestination;

    [Header("EXTRA HOLE")]
    [SerializeField] private GameObject extraHole;
    [SerializeField] private RectTransform extraHoleIconTransform;
    [SerializeField] private TMP_Text extraHoleAmountTmp;
    [SerializeField] private RectTransform extraHoleDestination;
    [Header("HAMMER")]
    [SerializeField] private GameObject hammer;
    [SerializeField] private RectTransform hammerIconTransform;
    [SerializeField] private TMP_Text hammerAmountTmp;
    [SerializeField] private RectTransform hammerDestination;
    [Header("DOUBLE TOOLBOX")]
    [SerializeField] private GameObject doubleToolbox;
    [SerializeField] private RectTransform doubleToolboxIconTransform;
    [SerializeField] private TMP_Text doubleToolboxAmountTmp;
    [SerializeField] private RectTransform doubleToolboxDestination;

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

    [Button(ButtonStyle.FoldoutButton)]
    public RewardAnimationGroup AddCoin(int amount)
    {
        working++;

        StartCoroutine(Cinematic());

        IEnumerator Cinematic()
        {
            coin.SetActive(true);
            coinAmountTmp.text = amount.ToString();
            coinIconTransform.localScale = Vector3.zero;
            coinIconTransform.DOScale(1f, fadeDuration);

            yield return waitUntilTriggered;

            coinIconTransform.DOScale(0f, fadeDuration);

            //coinParticle.onFirstParticleFinish.AddListener(() =>
            //{
            //    coinParticle.onFirstParticleFinish.RemoveAllListeners();
            //    DataManager.Ins.ChangeCoinAmount(amount, placement);
            //});

            //coinParticle.onLastParticleFinish.AddListener(() =>
            //{
            //    coinParticle.onLastParticleFinish.RemoveAllListeners();
            //    coin.SetActive(false);
            //    working--;
            //});

            //coinParticle.Play();
        }

        return this;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public RewardAnimationGroup AddInfiniteLife(int seconds)
    {
        working++;

        StartCoroutine(Cinematic());

        IEnumerator Cinematic()
        {
            infiniteLife.SetActive(true);
            infiniteLifeDurationTmp.gameObject.SetActive(true);
            infiniteLifeDurationTmp.text = GetDurationString(seconds);
            infiniteLifeIconTransform.localPosition = Vector3.zero;
            infiniteLifeIconTransform.localScale = Vector3.zero;
            infiniteLifeIconTransform.DOScale(1f, fadeDuration);

            yield return waitUntilTriggered;

            infiniteLifeDurationTmp.gameObject.SetActive(false);
            infiniteLifeIconTransform.DOMove(infiniteLifeDestination.position, particleFlyDuration).SetEase(Ease.InQuint);
            infiniteLifeIconTransform.DOScale(0.6f, particleFlyDuration);

            yield return waitParticle;

            //LifeManager.Ins.AddUnlimitedTime(seconds);
            infiniteLife.SetActive(false);
            working--;
        }

        return this;
    }

    public string GetDurationString(int seconds)
    {
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds / 60;
        seconds -= minutes * 60;

        string result = "";

        if (hours > 0) result += $"{hours}h";
        if (minutes > 0) result += $"{minutes}m";
        if (seconds > 0) result += $"{seconds}s";

        return result;
    }

    private void BoosterCinematic(
        GameObject booster, RectTransform icon, TMP_Text amountTmp,
        RectTransform destination, int amount, Action onComplete = null)
    {
        StartCoroutine(Cinematic());

        IEnumerator Cinematic()
        {
            booster.SetActive(true);
            amountTmp.gameObject.SetActive(true);
            amountTmp.text = $"x{amount}";
            icon.localPosition = Vector3.zero;
            icon.localScale = Vector3.zero;
            icon.DOScale(1f, fadeDuration);

            yield return waitUntilTriggered;

            amountTmp.gameObject.SetActive(false);
            icon.DOMove(destination.position, particleFlyDuration).SetEase(Ease.InQuint);
            icon.DOScale(0.6f, particleFlyDuration);

            yield return waitParticle;

            booster.SetActive(false);
            onComplete?.Invoke();
        }
    }

    [Button(ButtonStyle.FoldoutButton)]
    public RewardAnimationGroup AddExtraHole(int amount)
    {
        working++;

        BoosterCinematic(extraHole, extraHoleIconTransform,
            extraHoleAmountTmp, extraHoleDestination, amount, () =>
            {
                //DataManager.Ins.ChangeExtraHoleAmount(amount, placement);
                working--;
            });

        return this;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public RewardAnimationGroup AddHammer(int amount)
    {
        working++;

        BoosterCinematic(hammer, hammerIconTransform,
            hammerAmountTmp, hammerDestination, amount, () =>
            {
                //DataManager.Ins.ChangeHammerAmount(amount, placement);
                working--;
            });

        return this;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public RewardAnimationGroup AddDoubleToolbox(int amount)
    {
        working++;

        BoosterCinematic(doubleToolbox, doubleToolboxIconTransform,
            doubleToolboxAmountTmp, doubleToolboxDestination, amount, () =>
            {
                //DataManager.Ins.ChangeDoubleToolboxAmount(amount, placement);
                working--;
            });

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
