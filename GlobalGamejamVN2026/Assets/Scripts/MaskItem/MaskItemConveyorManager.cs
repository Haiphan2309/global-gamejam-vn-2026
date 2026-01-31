using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskItemConveyorManager : Singleton<MaskItemConveyorManager>
{
    [Header("References")]
    public MaskItemSpawner spawner;

    [Header("Area 1: Table (Background)")]
    public Transform tableSpawnPoint;

    [Header("Area 3: Next Item UI")]
    public Image nextItemImage;
    public TextMeshProUGUI nextItemText;

    private MaskItemVariant _nextItemBuffer;
    private GameObject _currentObjectOnTable;

    private void Start()
    {
        OnCustomerArrived(3,15);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveCurrentAndSpawnNext();
        }
    }

    public void OnCustomerArrived(int pimpleCount, int spawnWindow)
    {
        spawner.OnCustomerArrived(pimpleCount, spawnWindow);
        _nextItemBuffer = spawner.GetNextItemData();

        SpawnNextItemToTable();
    }

    public void RemoveCurrentAndSpawnNext()
    {
        if (_currentObjectOnTable != null)
        {
            Destroy(_currentObjectOnTable);
        }

        SpawnNextItemToTable();
    }

    private void SpawnNextItemToTable()
    {
        if (_nextItemBuffer == null) return;

        if (_nextItemBuffer.prefab != null)
        {
            _currentObjectOnTable = Instantiate(_nextItemBuffer.prefab, tableSpawnPoint.position, Quaternion.identity);
        }

        _nextItemBuffer = spawner.GetNextItemData();

        UpdateNextItemUI();
    }

    private void UpdateNextItemUI()
    {
        if (_nextItemBuffer != null && _nextItemBuffer.uiIcon != null)
        {
            nextItemImage.sprite = _nextItemBuffer.uiIcon;
            nextItemText.text = _nextItemBuffer.name;
            nextItemImage.enabled = true;
        }
        else
        {
            nextItemImage.enabled = false;
        }
    }
}