using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class TypeSpawnRate
{
    public MaskItemType itemType;
    [Range(0, 100)] public int weight;
}

public class MaskItemSpawner : MonoBehaviour
{
    [Header("Data Config")]
    public List<MaskItemDataSO> allItemDatas;

    public List<TypeSpawnRate> defaultTypeRates;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public int queueBufferSize = 100;
    public int firstGoodItemNumber = 2;
    [SerializeField] private MaskItemType fallbackItem = MaskItemType.CucumberSlice;


    // Private Runtime Data
    private Dictionary<MaskItemType, MaskItemDataSO> _dataLookup;
    private Queue<MaskItemType> _spawnQueue = new Queue<MaskItemType>();

    // Testing
    private int itemSpawnIndex = 0;

    private void Awake()
    {
        _dataLookup = new Dictionary<MaskItemType, MaskItemDataSO>();
        foreach (var data in allItemDatas)
        {
            if (!_dataLookup.ContainsKey(data.type))
                _dataLookup.Add(data.type, data);
        }
    }

    public MaskItemVariant GetNextItemData()
    {
        if (_spawnQueue.Count == 0)
        {
            FillQueue(defaultTypeRates, queueBufferSize);
        }

        MaskItemType typeToSpawn = _spawnQueue.Dequeue();

        if (_dataLookup.TryGetValue(typeToSpawn, out MaskItemDataSO dataSO))
        {
            var itemVariant = dataSO.GetRandomVariant();
            
            if (itemVariant != null)
            {
                GameObject prefab = itemVariant.prefab;
                Debug.Log($"Spawned : {prefab.name}");
                return itemVariant;
            }
        }
        return null;
    }

    private void FillQueue(List<TypeSpawnRate> rates, int count)
    {
        List<MaskItemType> newBatch = GenerateWeightedBatch(rates, count);

        foreach (var item in newBatch)
        {
            _spawnQueue.Enqueue(item);
        }
    }

    public void OnCustomerArrived(int pimpleCount, int spawnWindow)
    {
        _spawnQueue.Clear();

        List<MaskItemType> initialBatch = GenerateWeightedBatch(defaultTypeRates, queueBufferSize);

        SanitizeBatch(initialBatch, MaskItemType.AcnePatch);

        // Make sure 2 first items is cucumber slices
        if (initialBatch.Count >= firstGoodItemNumber)
        {
            initialBatch[0] = MaskItemType.CucumberSlice;
            initialBatch[1] = MaskItemType.CucumberSlice;
        }

        if (pimpleCount > 0)
        {
            int finalWindow = Mathf.Min(spawnWindow, queueBufferSize);

            InjectPriorityItems(initialBatch, MaskItemType.AcnePatch, pimpleCount, firstGoodItemNumber, finalWindow);
        }

        foreach (var type in initialBatch)
        {
            _spawnQueue.Enqueue(type);
        }
    }

    private void SanitizeBatch(List<MaskItemType> batch, MaskItemType typeToRemove)
    {
        for (int i = 0; i < batch.Count; i++)
        {
            if (batch[i] == typeToRemove)
            {
                // Thay thế bằng item mặc định (Dưa leo) để lấp chỗ trống
                batch[i] = fallbackItem; 
            }
        }
    }

    private void InjectPriorityItems(List<MaskItemType> batch, MaskItemType typeToInject, int count, int startIndex, int endIndex)
    {
        if (endIndex > batch.Count) endIndex = batch.Count;
        if (startIndex >= endIndex) return;

        List<int> availableIndexes = Enumerable.Range(startIndex, endIndex - startIndex).ToList();

        int amountToSpawn = Mathf.Min(count, availableIndexes.Count);

        for (int i = 0; i < amountToSpawn; i++)
        {
            int randIndexInList = Random.Range(0, availableIndexes.Count);
            int targetIndex = availableIndexes[randIndexInList];

            batch[targetIndex] = typeToInject;

            availableIndexes.RemoveAt(randIndexInList);
        }
    }

    private List<MaskItemType> GenerateWeightedBatch(List<TypeSpawnRate> rates, int count)
    {
        List<MaskItemType> result = new List<MaskItemType>();
        int totalWeight = rates.Sum(r => r.weight);

        for (int i = 0; i < count; i++)
        {
            int rnd = UnityEngine.Random.Range(0, totalWeight);
            int currentSum = 0;

            foreach (var rate in rates)
            {
                currentSum += rate.weight;
                if (rnd <= currentSum)
                {
                    result.Add(rate.itemType);
                    break;
                }
            }
        }
        return result;
    }
}