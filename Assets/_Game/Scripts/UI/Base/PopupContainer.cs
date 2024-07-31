#if ADDRESSABLE
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Panel Popup/Scriptable Objects/Popup Container")]
public class PopupContainer : ScriptableObject
{
    [System.Serializable]
    public struct PopupConfig
    {
        [HorizontalGroup("row", Width = 0.3f), HideLabel]
        public PopupId id;
        [HorizontalGroup("row", Width = 0.7f), HideLabel]
        public AssetReferenceGameObject reference;
    }

    [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 10)]
    PopupConfig[] configs;

    Dictionary<PopupId, AssetReferenceGameObject> referenceById;

    public AssetReferenceGameObject this[PopupId id]
    {
        get
        {
            if (configs.IsNullOrEmpty())
            {
                Debug.LogError("Container is empty");
                return null;
            }

            if (referenceById == null)
            {
                referenceById = new Dictionary<PopupId, AssetReferenceGameObject>();
            }

            if (referenceById.Count != configs.Length)
            {
                for (int i = 0; i < configs.Length; i++)
                {
                    referenceById.Add(configs[i].id, configs[i].reference);
                }
            }

            if (!referenceById.ContainsKey(id) || referenceById[id] == null)
            {
                Debug.LogError($"There is no reference corresponding to this id: {id}");

                return null;
            }

            return referenceById[id];
        }
    }
}
#endif
