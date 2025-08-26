using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    public AudioClip customClickSound;
    public bool playDefaultClickSound = true;
    
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    
    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }
    
    private void PlayClickSound()
    {
        if (SoundManager.Instance == null) return;
        
        if (customClickSound != null)
        {
            SoundManager.Instance.PlaySFX(customClickSound);
        }
        else if (playDefaultClickSound)
        {
            SoundManager.Instance.PlayButtonClick();
        }
    }
    
    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(PlayClickSound);
        }
    }
}