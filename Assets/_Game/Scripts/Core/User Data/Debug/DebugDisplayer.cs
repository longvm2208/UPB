using Sirenix.OdinInspector;
using UnityEngine;

public class DebugDisplayer : MonoBehaviour
{
    [SerializeField] private DebugMessage messagePrefab;

    private RectTransform myTransform;

    private void Awake()
    {
        myTransform = GetComponent<RectTransform>();
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void LogEvent(string name, params DebugParameter[] parameters)
    {
        if (Application.isEditor)
        {
            var message = messagePrefab.Spawn(myTransform);
            message.OnInit(name, parameters);
        }
    }
}
