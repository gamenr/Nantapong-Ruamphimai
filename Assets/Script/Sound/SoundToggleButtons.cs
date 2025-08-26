using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButtons : MonoBehaviour
{
    [Header("Music Button")]
    public Button musicButton;
    public Image musicButtonImage;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;
    
    [Header("SFX Button")]
    public Button sfxButton;
    public Image sfxButtonImage;
    public Sprite sfxOnIcon;
    public Sprite sfxOffIcon;
    
    [Header("Optional Colors")]
    public Color enabledColor = Color.white;
    public Color disabledColor = Color.gray;
    public bool useColorTinting = false;
    
    private void Start()
    {
        SetupButtons();
        UpdateButtonVisuals();
    }
    
    private void SetupButtons()
    {
        if (musicButton != null)
        {
            musicButton.onClick.AddListener(ToggleMusic);
        }
        
        if (sfxButton != null)
        {
            sfxButton.onClick.AddListener(ToggleSFX);
        }
    }
    
    public void ToggleMusic()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ToggleMusic();
            UpdateMusicButtonVisual();
        }
    }
    
    public void ToggleSFX()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ToggleSFX();
            UpdateSFXButtonVisual();
        }
    }
    
    public void UpdateButtonVisuals()
    {
        UpdateMusicButtonVisual();
        UpdateSFXButtonVisual();
    }
    
    private void UpdateMusicButtonVisual()
    {
        if (SoundManager.Instance == null || musicButtonImage == null) return;
        
        bool isEnabled = SoundManager.Instance.IsMusicEnabled;
        
        if (musicOnIcon != null && musicOffIcon != null)
        {
            musicButtonImage.sprite = isEnabled ? musicOnIcon : musicOffIcon;
        }
        
        if (useColorTinting)
        {
            musicButtonImage.color = isEnabled ? enabledColor : disabledColor;
        }
    }
    
    private void UpdateSFXButtonVisual()
    {
        if (SoundManager.Instance == null || sfxButtonImage == null) return;
        
        bool isEnabled = SoundManager.Instance.IsSFXEnabled;
        
        if (sfxOnIcon != null && sfxOffIcon != null)
        {
            sfxButtonImage.sprite = isEnabled ? sfxOnIcon : sfxOffIcon;
        }
        
        if (useColorTinting)
        {
            sfxButtonImage.color = isEnabled ? enabledColor : disabledColor;
        }
    }
    
    private void OnEnable()
    {
        UpdateButtonVisuals();
    }
    
    private void OnDestroy()
    {
        if (musicButton != null)
        {
            musicButton.onClick.RemoveListener(ToggleMusic);
        }
        
        if (sfxButton != null)
        {
            sfxButton.onClick.RemoveListener(ToggleSFX);
        }
    }
}