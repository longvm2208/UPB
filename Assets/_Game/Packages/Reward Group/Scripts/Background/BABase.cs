using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class BABase : MonoBehaviour
{
    protected bool canArrangeReward;

    WaitUntil waitUntilCanArrange;

    public WaitUntil WaitUntilCanArrange => waitUntilCanArrange;

    [Button(ButtonStyle.FoldoutButton)]
    public virtual void Initialize()
    {
        canArrangeReward = false;
        waitUntilCanArrange = new WaitUntil(() => canArrangeReward);
    }

    [Button(ButtonStyle.FoldoutButton)]
    public abstract void FadeIn(float duration);

    [Button(ButtonStyle.FoldoutButton)]
    public abstract void FadeOut(float duration);
}
