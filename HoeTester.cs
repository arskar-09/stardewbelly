using UnityEngine;

public class HoeTester : MonoBehaviour
{
    public FarmingTileManager farmingManager; // Inspector에서 연결

    void Update()
    {
        // 마우스 좌클릭 시 해당 타일 괭이질
        if (Input.GetMouseButtonDown(0))
        {
            // 1. 마우스의 화면 좌표를 게임 월드 좌표로 변환
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0; // 2D이므로 Z축 고정

            // 2. 월드 좌표를 타일맵 격자 좌표(Vector3Int)로 변환
            Vector3Int cellPos = farmingManager.groundLayer.WorldToCell(mouseWorldPos);

            // 3. 괭이질 실행
            bool success = farmingManager.HoeSoil(cellPos);
            if (success)
            {
                Debug.Log($"[괭이질 성공] {cellPos} 위치가 밭으로 변했습니다.");
            }
        }
    }
}
