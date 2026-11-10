using UnityEngine;

public class StardewPlayer : MonoBehaviour
{
    public float speed = 4f;
    private GameObject cursor;

    void Start()
    {
        // 캐릭터 외형 생성
        var sr = gameObject.AddComponent<SpriteRenderer>();
        sr.color = Color.blue; // 임시 캐릭터
        
        // 커서(주황색 칸) 생성
        cursor = new GameObject("Cursor");
        var cursorSr = cursor.AddComponent<SpriteRenderer>();
        cursorSr.color = new Color(1f, 0.5f, 0f, 0.5f); // 주황색 반투명
        // 1x1 도트 텍스처를 커서로 사용
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(h, v, 0) * speed * Time.deltaTime;

        // 스타듀 밸리식 그리드 커서 위치 계산
        Vector3Int tilePos = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y - 0.5f),
            0
        );
        cursor.transform.position = (Vector3)tilePos + new Vector3(0.5f, 0.5f, 0);
    }
}
