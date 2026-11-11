using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmingTileManager : MonoBehaviour
{
    [Header("타일맵 레이어")]
    public Tilemap groundLayer;   // 기본 바닥 레이어
    public Tilemap farmingLayer;  // 경작지 레이어 (Ground 위에 얹어짐)

    [Header("경작 타일 에셋")]
    public TileBase tilledSoilTile;  // 괭이질된 흙 타일 (Rule Tile 권장)
    public TileBase wateredSoilTile; // 물 준 흙 타일

    /// <summary>
    /// [맵 담당 로직] 전달받은 좌표의 땅을 괭이로 갈아 밭으로 만듭니다.
    /// </summary>
    public bool HoeSoil(Vector3Int cellPosition)
    {
        // 1. 바닥에 흙(Dirt) 타일이 있는지 확인
        TileBase groundTile = groundLayer.GetTile(cellPosition);

        if (groundTile != null && groundTile.name.Contains("Dirt"))
        {
            // 2. 아직 경작되지 않은 곳이라면 밭 타일을 배치
            if (!farmingLayer.HasTile(cellPosition))
            {
                farmingLayer.SetTile(cellPosition, tilledSoilTile);
                return true; // 성공
            }
        }
        return false; // 경작 불가 지역
    }

    /// <summary>
    /// [맵 담당 로직] 밭에 물을 줍니다.
    /// </summary>
    public void WaterSoil(Vector3Int cellPosition)
    {
        if (farmingLayer.HasTile(cellPosition))
        {
            farmingLayer.SetTile(cellPosition, wateredSoilTile);
        }
    }

    /// <summary>
    /// [맵 담당 로직] 밭을 다시 일반 땅으로 원상복구합니다.
    /// </summary>
    public void ClearSoil(Vector3Int cellPosition)
    {
        farmingLayer.SetTile(cellPosition, null);
    }
}
