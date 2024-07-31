using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
#if ADDRESSABLE
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

public class UIManager : SingletonMonoBehaviour<UIManager>
{
#if UNITY_EDITOR
    [SerializeField, ExposedScriptableObject]
    RamManagementConfig RamManagementConfig;
#endif
    [SerializeField] PanelBase panel;
    [SerializeField] RectTransform popupParent;
    [SerializeField] GameObject blocker;
#if ADDRESSABLE
    [SerializeField, ExposedScriptableObject]
    PopupContainer container;
#endif

    int blockCount = 0;
    int popupOpenCount = 0;
    int panelDisableCount = 0;
    Dictionary<PopupId, PopupBase> popupById = new();
#if ADDRESSABLE
    List<AsyncOperationHandle<GameObject>> operationHandles = new();
#endif

    public bool HasOpenPopup => popupOpenCount > 0;
    public PanelBase Panel => panel;

#if ADDRESSABLE
    void OnDestroy()
    {
        if (!operationHandles.IsNullOrEmpty())
        {
            for (int i = 0; i < operationHandles.Count; i++)
            {
                Addressables.Release(operationHandles[i]);
            }
        }
    }
#endif

    #region PANEL
    public void EnablePanel()
    {
        panelDisableCount--;
        panel.EnableCanvas(panelDisableCount <= 0);
    }

    public void DisablePanel()
    {
        panelDisableCount++;
        panel.EnableCanvas(panelDisableCount <= 0);
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

#if ADDRESSABLE
    public bool IsLoadingAsset(PopupId id)
    {
        return !container[id].IsDone;
    }
#endif

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
#if ADDRESSABLE
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
#elif RESOURCES
        if (!IsPopupInstantiated(id))
        {
            PopupBase prefab = Resources.Load<PopupBase>("Popup" + id.ToString());
            popupById[id] = Instantiate(prefab, popupParent);
        }

        popupById[id].Open(args);
#endif
    }

#if ADDRESSABLE
    void InstantiatePopup(PopupId id, object args = null)
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
#endif
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
