#if UNITY_EDITOR
using System.ComponentModel;
using UnityEngine;

public enum RamManagementType
{
    [HideInInspector]
    None = -1,
    [Description("ADDRESSABLE")]
    Addressable,
    [Description("RESOURCES")]
    Resource,
}
#endif
