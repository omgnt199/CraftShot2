using EditorAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class VSEquipment : ScriptableObject
{
    public VSEquipmentPool Pool;
    public VSEquipmentType Type;
    public string Name;
    public Sprite Icon;
    public GameObject Model;
    public GameObject OnlyModel;
    public GameObject ModelForBot;
    public AnimatorOverrideController AnimatorControllerForBot;
    public CurrencyType Currency;
    public int Price;
#if UNITY_EDITOR
    [Button]
    public void InitializePool()
    {
        if (!Pool.EquipmentList.Contains(this)) Pool.EquipmentList.Add(this);
        else Debug.Log("Initialized");
    }
    [Button]
    public void RenameAssetFile()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, "Equipment " + Name);

    }
#endif
}
