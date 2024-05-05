#define ADDRESSABLE
//#define RESOURCES

#if ADDRESSABLE
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private PanelBase panel;
    [SerializeField] private RectTransform popupParent;
    [SerializeField] private GameObject blocker;
    [SerializeField, ExposedScriptableObject]
    private PopupContainer container;

    private int blockCount = 0;
    private int popupOpenCount = 0;
    private int panelDisableCount = 0;
    private Dictionary<PopupId, PopupBase> popupById = new();
    private List<AsyncOperationHandle<GameObject>> operationHandles = new();

    public bool HasOpenPopup => popupOpenCount > 0;
    public PanelBase Panel => panel;

    private void OnDestroy()
    {
        if (!operationHandles.IsNullOrEmpty())
        {
            for (int i = 0; i < operationHandles.Count; i++)
            {
                Addressables.Release(operationHandles[i]);
            }
        }
    }

    #region PANEL
    public void EnablePanel()
    {
        panelDisableCount--;
        panel.gameObject.SetActive(panelDisableCount <= 0);
    }

    public void DisablePanel()
    {
        panelDisableCount++;
        panel.gameObject.SetActive(panelDisableCount <= 0);
    }
    #endregion

    #region POPUP
    public void OnPopupOpen()
    {
        popupOpenCount++;
    }

    public void OnPopupClose()
    {
        popupOpenCount--;
    }

    public bool IsPopupInstantiated(PopupId id)
    {
        return popupById.ContainsKey(id) && popupById[id] != null;
    }

    public bool IsPopupOpen(PopupId id)
    {
        return IsPopupInstantiated(id) && popupById[id].gameObject.activeSelf;
    }

    public bool IsLoadingAsset(PopupId id)
    {
        return !container[id].IsDone;
    }

    public PopupBase Popup(PopupId id)
    {
        if (!IsPopupOpen(id))
        {
            Debug.LogError($"Need to open popup first: {id}");

            return null;
        }

        return popupById[id];
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void OpenPopup(PopupId id, object args = null)
    {
        if (IsPopupInstantiated(id))
        {
            PopupBase popup = popupById[id];

            if (popup.gameObject.activeSelf)
            {
                Debug.LogWarning($"Popup {id} is open");
                return;
            }

            popup.Open(args);
        }
        else
        {
            InstantiatePopup(id, args);
        }
    }

    private void InstantiatePopup(PopupId id, object args = null)
    {
        var reference = container[id];

        if (reference == null)
        {
            Debug.LogError($"Reference is null: {id}");
            return;
        }

        if (!reference.IsDone)
        {
            Debug.LogWarning($"Loading asset: {id}");
            return;
        }

        var operationHandle = reference.LoadAssetAsync();
        operationHandles.Add(operationHandle);

        EnableBlocker();

        operationHandle.Completed += (handle) =>
        {
            DisableBlocker();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                PopupBase popup = Instantiate(handle.Result, popupParent).GetComponent<PopupBase>();
                popup.Open(args);
                popupById[id] = popup;
            }
            else
            {
                Debug.LogError($"Cannot load asset: {handle.Status}");
            }
        };
    }
    #endregion

    #region BLOCKER
    public void EnableBlocker()
    {
        blockCount++;
        blocker.SetActive(blockCount > 0);
    }

    public void DisableBlocker()
    {
        blockCount--;
        blocker.SetActive(blockCount > 0);
    }
    #endregion
}
#elif RESOURCES
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private PanelBase panel;
    [SerializeField] private RectTransform popupParent;
    [SerializeField] private GameObject blocker;

    private int blockCount = 0;
    private int popupOpenCount = 0;
    private int panelDisableCount = 0;
    private Dictionary<PopupId, PopupBase> popupById = new();

    public bool HasOpenPopup => popupOpenCount > 0;
    public PanelBase Panel => panel;

    #region PANEL
    public void EnablePanel()
    {
        panelDisableCount--;
        panel.gameObject.SetActive(panelDisableCount <= 0);
    }

    public void DisablePanel()
    {
        panelDisableCount++;
        panel.gameObject.SetActive(panelDisableCount <= 0);
    }
    #endregion

    #region POPUP
    public void OnPopupOpen()
    {
        popupOpenCount++;
    }

    public void OnPopupClose()
    {
        popupOpenCount--;
    }

    public bool IsPopupInstantiated(PopupId id)
    {
        return popupById.ContainsKey(id) && popupById[id] != null;
    }

    public bool IsPopupOpen(PopupId id)
    {
        return IsPopupInstantiated(id) && popupById[id].gameObject.activeSelf;
    }

    public PopupBase Popup(PopupId id)
    {
        if (!IsPopupOpen(id))
        {
            Debug.LogError($"Need to open popup first: {id}");

            return null;
        }

        return popupById[id];
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void OpenPopup(PopupId id, object args = null)
    {
        if (!IsPopupInstantiated(id))
        {
            PopupBase prefab = Resources.Load<PopupBase>("Popup" + id.ToString());
            popupById[id] = Instantiate(prefab, popupParent);
        }

        popupById[id].Open(args);
    }
    #endregion

    #region BLOCKER
    public void EnableBlocker()
    {
        blockCount++;
        blocker.SetActive(blockCount > 0);
    }

    public void DisableBlocker()
    {
        blockCount--;
        blocker.SetActive(blockCount > 0);
    }
    #endregion
}
#endif
