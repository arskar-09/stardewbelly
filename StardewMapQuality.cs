using UnityEngine;

public class StardewMapQuality : MonoBehaviour
{
    void Start()
    {
        // 1. 스프라이트 렌더러를 가져옵니다.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        if (sr != null)
        {
            // 2. 스타듀 밸리처럼 도트가 튀지 않게 설정
            sr.spriteSortPoint = SpriteSortPoint.Pivot;
            
            // 3. 재질을 스프라이트 전용 기본 재질로 설정 (더 선명함)
            sr.material = new Material(Shader.Find("Sprites/Default"));
        }
    }
}
