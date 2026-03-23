using System.Collections;
using Pool;
using UnityEngine;

/// <summary>
/// StonePlane 게임오브젝트에 부착.
/// 5x10 그리드로 돌을 배치하고, 채굴된 돌을 일정 시간 후 재생성합니다.
/// </summary>
public class StoneQuarry : MonoBehaviour
{
    [SerializeField] private int columns = 7;
    [SerializeField] private int rows = 14;
    [SerializeField] private float spacing = 1.43f;
    [SerializeField] private float respawnDelay = 10f;

    private const string PoolKey = "stone";

    private Vector3[] _slotPositions;
    private Stone[] _slots;

    private void Start()
    {
        int total = columns * rows;
        _slotPositions = new Vector3[total];
        _slots = new Stone[total];

        float offsetX = (columns - 1) * spacing * 0.5f;
        float offsetZ = (rows - 1) * spacing * 0.5f;

        int index = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                _slotPositions[index] = transform.position
                    + transform.right   * (col * spacing - offsetX)
                    + transform.forward * (row * spacing - offsetZ)
                    + Vector3.up        * 0.5f;

                SpawnStoneAt(index);
                index++;
            }
        }
    }

    private void SpawnStoneAt(int index)
    {
        Stone stone = PoolManager.Instance.Get<Stone>(PoolKey);
        stone.transform.SetPositionAndRotation(_slotPositions[index], transform.rotation);
        stone.Initialize(this, index);
        _slots[index] = stone;
    }

    /// <summary>Stone.OnDeactivated() 에서 호출됩니다.</summary>
    public void OnStoneMined(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Length) return;
        _slots[slotIndex] = null;
        StartCoroutine(RespawnCoroutine(slotIndex));
    }

    private IEnumerator RespawnCoroutine(int index)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnStoneAt(index);
    }
}
