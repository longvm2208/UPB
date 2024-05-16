using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class DebugMessage : MonoBehaviour
{
    private const string Event = "<color=#00FF00>- [{0}] </color>";
    private const string Param = "<color=#00FFFF> [{0} - {1}] </color>";
    private static WaitForSeconds wait = new WaitForSeconds(3f);

    [SerializeField] private RectTransform myTransform;
    [SerializeField] private TMP_Text tmp;

    private PrefabInstancePool<DebugMessage> pool;

    public RectTransform MyTransform => myTransform;

    private void OnValidate()
    {
        myTransform = transform as RectTransform;
        tmp = GetComponent<TMP_Text>();
    }

    public DebugMessage Spawn(RectTransform parent)
    {
        var instance = pool.GetInstance(this);
        instance.pool = pool;
        instance.MyTransform.SetParent(parent);
        return instance;
    }

    public void Despawn()
    {
        pool.Recycle(this);
    }

    public void OnInit(string name, params DebugParameter[] parameters)
    {
        string message = string.Format(Event, name);

        foreach (var parameter in parameters)
        {
            message += string.Format(Param, parameter.name, parameter.value);
        }

        myTransform.SetAsFirstSibling();
        tmp.text = message;
        tmp.ChangeAlpha(1f);

        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return wait;
            tmp.DOFade(0f, 1f).OnComplete(Despawn);
        }
    }
}
