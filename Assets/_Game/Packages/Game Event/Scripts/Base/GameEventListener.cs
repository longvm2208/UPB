using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;

    private void OnEnable()
    {
        if (gameEvent == null)
        {
            return;
        }

        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null)
        {
            return;
        }

        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response?.Invoke();
    }
}
