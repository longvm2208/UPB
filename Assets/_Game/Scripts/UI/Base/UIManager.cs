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
    [SerializeField]
    private PanelBase panel;
    [SerializeField]
    private RectTransform popupParent;
    [SerializeField]
    private GameObject blocker;
    [SerializeField, ExposedScriptableObject]
    private PopupContainer container;

    private int blockCount = 0;
    private int popupOpenCount = 0;
    private int panelDisableCount = 0;
    private Dictionary<PopupId, PopupBase> popupById;
    private List<AsyncOperationHandle<GameObject>> operationHandles;

    public bool HasOpenPopup => popupOpenCount > 0;

    private void OnDestroy()
    {
        ReleasePopupReference();
    }

    #region PANEL
    public T Panel<T>() where T : PanelBase => panel as T;

    public void EnablePanel()
    {
        if (--panelDisableCount < 0)
        {
            panelDisableCount = 0;
        }

        if (panelDisableCount == 0)
        {
            panel.gameObject.SetActive(true);
        }
    }

    public void DisablePanel()
    {
        if (panelDisableCount == 0)
        {
            panel.gameObject.SetActive(false);
        }

        panelDisableCount++;
    }
    #endregion

    #region POPUP
    public bool IsPopupInstantiated(PopupId id)
    {
        return popupById.ContainsKey(id) && popupById[id] != null;
    }

    public bool IsPopupOpen(PopupId id)
    {
        return IsPopupInstantiated(id) && popupById[id].IsOpen();
    }

    public bool IsLoadingAsset(PopupId id)
    {
        return !container[id].IsDone;
    }

    private T Popup<T>(PopupId id) where T : PopupBase
    {
        if (IsPopupOpen(id))
        {
            return popupById[id] as T;
        }
        else
        {
            Debug.LogError($"Need to open popup first: {id}");

            return null;
        }
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void OpenPopup(PopupId id, object args = null)
    {
        if (popupById == null || operationHandles == null)
        {
            popupById = new();
            operationHandles = new();
        }

        if (IsPopupInstantiated(id))
        {
            PopupBase popup = popupById[id];

            if (popup.IsOpen())
            {
                Debug.LogWarning($"Popup {id} is opening");
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

    public void OnPopupOpen()
    {
        popupOpenCount++;
    }

    public void OnPopupClose()
    {
        popupOpenCount--;
    }

    private void ReleasePopupReference()
    {
        if (operationHandles.IsNullOrEmpty()) return;

        for (int i = 0; i < operationHandles.Count; i++)
        {
            Addressables.Release(operationHandles[i]);
        }
    }
    #endregion

    #region BLOCKER
    public void EnableBlocker()
    {
        if (blockCount == 0)
        {
            blocker.SetActive(true);
        }

        blockCount++;
    }

    public void DisableBlocker()
    {
        if (--blockCount < 0)
        {
            blockCount = 0;
        }

        if (blockCount == 0)
        {
            blocker.SetActive(false);
        }
    }
    #endregion
}
#elif RESOURCES
//using Sirenix.OdinInspector;
//using System.Collections.Generic;
//using UnityEngine;

//public class UIManager : SingletonMonoBehaviour<UIManager>
//{
//    [SerializeField] private PanelBase panel;
//    [SerializeField] private RectTransform popupParent;
//    [SerializeField] private GameObject blocker;

//    private int blockCount = 0;
//    private int popupOpenCount = 0;
//    private int panelDisableCount = 0;
//    private Dictionary<PopupId, PopupBase> popupById;

//    #region PANEL
//    public T Panel<T>() where T : PanelBase => panel as T;

//    public void EnablePanel()
//    {
//        if (--panelDisableCount < 0)
//        {
//            panelDisableCount = 0;
//        }

//        if (panelDisableCount == 0)
//        {
//            panel.gameObject.SetActive(true);
//        }
//    }

//    public void DisablePanel()
//    {
//        if (panelDisableCount == 0)
//        {
//            panel.gameObject.SetActive(false);
//        }

//        panelDisableCount++;
//    }
//    #endregion

//    #region POPUP
//    public bool IsPopupInstantiated(PopupId id)
//    {
//        return popupById.ContainsKey(id) && popupById[id] != null;
//    }

//    public bool IsPopupOpen(PopupId id)
//    {
//        return IsPopupInstantiated(id) && popupById[id].IsOpen();
//    }

//    private T Popup<T>(PopupId id) where T : PopupBase
//    {
//        if (IsPopupOpen(id))
//        {
//            return popupById[id] as T;
//        }
//        else
//        {
//            Debug.LogError($"Need to open popup first: {id}");

//            return null;
//        }
//    }

//    [Button(ButtonStyle.FoldoutButton)]
//    public void OpenPopup(PopupId id, object args = null)
//    {
//        if (popupById == null)
//        {
//            popupById = new();
//        }

//        if (!popupById.ContainsKey(id) || popupById[id] == null)
//        {
//            PopupBase prefab = Resources.Load<PopupBase>("Popup" + id.ToString());
//            popupById[id] = Instantiate(prefab, popupParent);
//        }

//        popupById[id].Open(args);
//    }

//    public void OnPopupOpen()
//    {
//        popupOpenCount++;
//    }

//    public void OnPopupClose()
//    {
//        popupOpenCount--;
//    }

//    public bool HasOpenPopup()
//    {
//        return popupOpenCount > 0;
//    }
//    #endregion

//    #region BLOCKER
//    public void EnableBlocker()
//    {
//        if (blockCount == 0)
//        {
//            blocker.SetActive(true);
//        }

//        blockCount++;
//    }

//    public void DisableBlocker()
//    {
//        if (--blockCount < 0)
//        {
//            blockCount = 0;
//        }

//        if (blockCount == 0)
//        {
//            blocker.SetActive(false);
//        }
//    }
//    #endregion
//}
#endif
