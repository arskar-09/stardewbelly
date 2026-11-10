using UnityEngine;
using UnityEngine.Rendering;

public class StardewDepthFix : MonoBehaviour
{
    void Start()
    {
        // 맵 전체에 스타듀 밸리식 앞뒤 정렬법을 강제로 적용합니다.
        // Y값이 낮은 물체가 더 앞에 보이게 하는 마법의 코드입니다.
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0, 1, 0);
    }
}
