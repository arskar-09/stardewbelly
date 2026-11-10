using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class StardewMapBuilder : MonoBehaviour
{
    private Tilemap groundMap;
    private Tilemap objectMap;

    void Start()
    {
        SetupCamera();
        SetupProjectSettings();
        CreateMapLayers();
        GenerateFarmMap();
    }

    void SetupCamera()
    {
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 7;
        cam.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        cam.transform.position = new Vector3(8, -5, -10); // 맵 중앙을 보도록 조정
    }

    void SetupProjectSettings()
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0, 1, 0);
    }

    void CreateMapLayers()
    {
        GameObject gridObj = new GameObject("StardewGrid");
        gridObj.AddComponent<Grid>();

        // 바닥 레이어
        GameObject groundObj = new GameObject("GroundLayer");
        groundObj.transform.parent = gridObj.transform;
        groundMap = groundObj.AddComponent<Tilemap>();
        groundObj.AddComponent<TilemapRenderer>().material = new Material(Shader.Find("Sprites/Default"));

        // 오브젝트 레이어 (나무, 돌 등)
        GameObject objLayer = new GameObject("ObjectLayer");
        objLayer.transform.parent = gridObj.transform;
        objectMap = objLayer.AddComponent<Tilemap>();
        var tr = objLayer.AddComponent<TilemapRenderer>();
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.mode = TilemapRenderer.Mode.Individual; // Y-Sorting 핵심 설정
        tr.sortingOrder = 1;
    }

    void GenerateFarmMap()
    {
        // 1. 에셋 생성 (단순 색상이 아닌 텍스처)
        Tile grass = CreateDetailTile(new Color(0.44f, 0.67f, 0.21f), "Grass");
        Tile dirt = CreateDetailTile(new Color(0.74f, 0.55f, 0.31f), "Dirt");
        Tile water = CreateDetailTile(new Color(0.24f, 0.52f, 0.78f), "Water");

        // 2. 바닥 깔기 (이미지 재현)
        for (int x = 0; x < 25; x++) {
            for (int y = -15; y < 0; y++) {
                // 기본은 잔디, 랜덤하게 흙
                Tile baseTile = (Random.value > 0.15f) ? grass : dirt;
                
                // 오른쪽 하단에 연못 생성
                if (x > 18 && y < -10) baseTile = water;

                groundMap.SetTile(new Vector3Int(x, y, 0), baseTile);

                // 3. 나무와 돌 배치 (오브젝트 레이어)
                if (baseTile == grass && Random.value < 0.12f) {
                    float spawnType = Random.value;
                    if (spawnType < 0.4f) CreateSpriteObject("Tree", x, y, new Color(0.13f, 0.35f, 0.13f), new Vector2(1, 2));
                    else if (spawnType < 0.7f) CreateSpriteObject("Rock", x, y, Color.gray, new Vector2(0.8f, 0.7f));
                    else CreateSpriteObject("Bush", x, y, new Color(0.2f, 0.5f, 0.2f), new Vector2(0.9f, 0.8f));
                }
            }
        }
    }

    // 디테일한 타일 생성 (점 찍기)
    Tile CreateDetailTile(Color baseColor, string name)
    {
        Texture2D tex = new Texture2D(16, 16);
        tex.filterMode = FilterMode.Point;
        for (int x = 0; x < 16; x++) {
            for (int y = 0; y < 16; y++) {
                // 노이즈 추가
                Color c = baseColor;
                if (Random.value > 0.8f) c *= 0.9f;
                if (Random.value > 0.95f) c *= 1.1f;
                tex.SetPixel(x, y, c);
            }
        }
        tex.Apply();
        Sprite sp = Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16);
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sp;
        return tile;
    }

    // 나무나 바위 같은 스프라이트 오브젝트 생성
    void CreateSpriteObject(string type, int x, int y, Color color, Vector2 size)
    {
        GameObject obj = new GameObject(type);
        obj.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
        
        var sr = obj.AddComponent<SpriteRenderer>();
        sr.color = color;
        sr.spriteSortPoint = SpriteSortPoint.Pivot; // Y-Sorting 포인트

        // 도트 텍스처 생성
        int w = (int)(16 * size.x);
        int h = (int)(16 * size.y);
        Texture2D tex = new Texture2D(w, h);
        tex.filterMode = FilterMode.Point;
        for (int i = 0; i < w * h; i++) tex.SetPixel(i % w, i / w, color * (1 - Random.value * 0.2f));
        tex.Apply();

        // 피벗을 아래로 설정하여 나무 뒤로 캐릭터가 갈 수 있게 함
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0f), 16);
    }
}
