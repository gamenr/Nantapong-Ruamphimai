using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFix : MonoBehaviour
{
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    private RectTransform rectTransform;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        ApplySafeArea();
    }

    void Update()
    {
        
        if (lastSafeArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        
       
        lastSafeArea = safeArea;
        
        
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        
       
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        
        
        anchorMin.x = Mathf.Clamp01(anchorMin.x);
        anchorMin.y = Mathf.Clamp01(anchorMin.y);
        anchorMax.x = Mathf.Clamp01(anchorMax.x);
        anchorMax.y = Mathf.Clamp01(anchorMax.y);
        
        
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        
        
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        if (showDebugInfo)
        {
            Debug.Log($"Safe Area Applied: Screen({Screen.width}x{Screen.height}) SafeArea({safeArea}) Anchors({anchorMin} to {anchorMax})");
        }
    }
}