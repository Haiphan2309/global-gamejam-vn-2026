using System.Collections.Generic;
using DG.Tweening;
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

    private MaskItemVariant _nextItemBuffer;
    private GameObject _currentObjectOnTable;

    public void OnCustomerArrived(int pimpleCount, int spawnWindow)
    {
        spawner.OnCustomerArrived(pimpleCount, spawnWindow);
        _nextItemBuffer = spawner.GetNextItemData();

        SpawnNextItemToTable();
    }

    public void NotifyItemPickedUp(GameObject itemPicked)
    {
        // Kiểm tra xem item đang kéo có đúng là item đang nằm trên bàn không
        if (_currentObjectOnTable == itemPicked)
        {
            // 1. Tách item này ra (quên nó đi), để nó trở thành vật thể tự do
            _currentObjectOnTable = null;

            // 2. Ngay lập tức spawn item kế tiếp vào bàn (để người chơi thấy item mới xuất hiện liền)
            SpawnNextItemToTable();
        }
    }

    private void SpawnNextItemToTable()
    {
        if (_currentObjectOnTable != null) return;
        if (_nextItemBuffer == null) return;

        if (_nextItemBuffer.prefab != null)
        {
            _currentObjectOnTable = Instantiate(_nextItemBuffer.prefab, tableSpawnPoint.position, Quaternion.identity);
            _currentObjectOnTable.transform.SetParent(tableSpawnPoint);
            _currentObjectOnTable.transform.DOMoveX(tableSpawnPoint.position.x + 3, 1.0f);
        }

        _nextItemBuffer = spawner.GetNextItemData();

        UpdateNextItemUI();
    }

    private void UpdateNextItemUI()
    {
        if (_nextItemBuffer != null && _nextItemBuffer.uiIcon != null)
        {
            nextItemImage.sprite = _nextItemBuffer.uiIcon;
            nextItemImage.enabled = true;
        }
        else
        {
            nextItemImage.enabled = false;
        }
    }
}