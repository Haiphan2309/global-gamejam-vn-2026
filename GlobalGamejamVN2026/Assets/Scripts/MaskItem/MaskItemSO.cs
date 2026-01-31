using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mask Item Data", menuName = "Game Data/Mask Item Data")]
public class MaskItemDataSO : ScriptableObject
{
    public string itemName;
    public MaskItemType type; 
    public bool isHarmfulEffect;
    
    [Header("Base Stats")]
    public float baseScorePerUnit;
    
    [Header("Visual Variants")]
    // Variants: Cucumber_Small, Cucumber_Big, Cucumber_Long,.....
    public List<MaskItemVariant> variants;
    

    public MaskItemVariant GetRandomVariant() 
    {
        if (variants == null || variants.Count == 0) return null;

        int totalWeight = variants.Sum(v => v.spawnWeight);
        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int currentSum = 0;

        foreach (var variant in variants)
        {
            currentSum += variant.spawnWeight;
            if (randomValue <= currentSum)
            {
                return variant;
            }
        }
        return variants[0];
    }
}

[Serializable]
public class MaskItemVariant
{
    public string name;
    public GameObject prefab;
    public Sprite uiIcon;
    [Range(0, 100)] public int spawnWeight;
}

[Serializable]
public enum MaskItemType
{
    CucumberSlice = 0,
    AcnePatch = 1,
    ChiliSlice = 2
}
