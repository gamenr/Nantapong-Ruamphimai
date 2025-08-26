using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class ShopTutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject tutorialOverlay; 
    public GameObject additionalOverlay; 

    [Header("All UI Buttons")]
    public List<Button> allButtons; 

    [System.Serializable]
    public class TutorialText
    {
        public TextMeshProUGUI textUI; 
        [TextArea] public string message;
    }

    [System.Serializable]
    public class TutorialStep
    {
        public List<TutorialText> texts; 
        public Button targetButton;      
        public bool useAdditionalOverlay = false; 
        public bool clearPreviousTexts = true; 
        public bool hideTargetButton = false; 
    }

    [Header("Tutorial Steps")]
    public List<TutorialStep> tutorialSteps;

    private int currentStepIndex = -1;
    private GameObject currentHighlightGO;
    private Dictionary<Button, bool> originalButtonStates = new Dictionary<Button, bool>();
    private Dictionary<Button, bool> originalButtonVisibility = new Dictionary<Button, bool>(); 
    private List<TextMeshProUGUI> allTextUIs = new List<TextMeshProUGUI>(); 
    private Coroutine pulseCoroutine;

    void Start()
    {
        InitializeTutorial();
    }

    void InitializeTutorial()
    {
        if (allButtons == null || allButtons.Count == 0)
        {
            FindAllButtons();
        }

        CollectAllTextUIs();
        SaveOriginalButtonVisibility();

        
        if (tutorialOverlay != null)
            tutorialOverlay.SetActive(false);
        
        if (additionalOverlay != null)
            additionalOverlay.SetActive(false);

        StartTutorial();
    }

    void CollectAllTextUIs()
    {
        allTextUIs.Clear();
        foreach (var step in tutorialSteps)
        {
            foreach (var text in step.texts)
            {
                if (text.textUI != null && !allTextUIs.Contains(text.textUI))
                {
                    allTextUIs.Add(text.textUI);
                }
            }
        }
    }

    void FindAllButtons()
    {
        Button[] foundButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        allButtons = new List<Button>(foundButtons);
    }

    void SaveOriginalButtonVisibility()
    {
        originalButtonVisibility.Clear();
        foreach (var step in tutorialSteps)
        {
            if (step.targetButton != null && !originalButtonVisibility.ContainsKey(step.targetButton))
            {
                originalButtonVisibility[step.targetButton] = step.targetButton.gameObject.activeInHierarchy;
            }
        }
    }

    void HideAllTargetButtons()
    {
        foreach (var step in tutorialSteps)
        {
            if (step.targetButton != null && step.hideTargetButton)
            {
                step.targetButton.gameObject.SetActive(false);
            }
        }
    }

    void DisableAllButtons()
    {
        originalButtonStates.Clear();

        foreach (Button button in allButtons)
        {
            if (button != null)
            {
                originalButtonStates[button] = button.interactable;
                
                if (currentStepIndex >= 0 && currentStepIndex < tutorialSteps.Count)
                {
                    button.interactable = (button == tutorialSteps[currentStepIndex].targetButton);
                }
                else
                {
                    button.interactable = false;
                }
            }
        }
    }

    void RestoreAllButtons()
    {
        
        foreach (var kvp in originalButtonStates)
        {
            if (kvp.Key != null)
            {
                kvp.Key.interactable = kvp.Value;
            }
        }
        originalButtonStates.Clear();

        
        foreach (var kvp in originalButtonVisibility)
        {
            if (kvp.Key != null)
            {
                kvp.Key.gameObject.SetActive(kvp.Value);
            }
        }
    }

    public void StartTutorial()
    {
        currentStepIndex = -1;
        SetupOverlayBlocking();
        HideAllTargetButtons(); 
        NextStep();
    }

    void SetupOverlayBlocking()
    {
        if (tutorialOverlay != null)
        {
            Image overlayImage = tutorialOverlay.GetComponent<Image>();
            if (overlayImage != null)
            {
                overlayImage.raycastTarget = true;
                overlayImage.color = new Color(0f, 0f, 0f, 0.5f);
            }
        }

        if (additionalOverlay != null)
        {
            Image additionalImage = additionalOverlay.GetComponent<Image>();
            if (additionalImage != null)
            {
                additionalImage.raycastTarget = true;
                additionalImage.color = new Color(0f, 0f, 0f, 0.5f);
            }
        }
    }

    void NextStep()
    {
        ClearTargetButtonListener();
        HidePreviousStepTargetButton();

        currentStepIndex++;

        if (currentStepIndex < tutorialSteps.Count)
        {
            ShowStep(tutorialSteps[currentStepIndex]);
        }
        else
        {
            EndTutorial();
        }
    }

    void ClearTargetButtonListener()
    {
        if (currentStepIndex >= 0 && currentStepIndex < tutorialSteps.Count)
        {
            var previousStep = tutorialSteps[currentStepIndex];
            if (previousStep.targetButton != null)
            {
                previousStep.targetButton.onClick.RemoveListener(OnTargetButtonClicked);
            }
        }
    }

    void HidePreviousStepTargetButton()
    {
        if (currentStepIndex >= 0 && currentStepIndex < tutorialSteps.Count)
        {
            var previousStep = tutorialSteps[currentStepIndex];
            if (previousStep.targetButton != null && previousStep.hideTargetButton)
            {
                previousStep.targetButton.gameObject.SetActive(false);
            }
        }
    }

    void ClearAllTutorialTexts()
    {
        foreach (var textUI in allTextUIs)
        {
            if (textUI != null)
            {
                textUI.text = "";
            }
        }
    }

    void ShowStep(TutorialStep step)
    {
        
        if (step.targetButton != null)
        {
            step.targetButton.gameObject.SetActive(true);
        }

        DisableAllButtons();

        if (step.clearPreviousTexts)
        {
            ClearAllTutorialTexts();
        }

       
        if (tutorialOverlay != null)
            tutorialOverlay.SetActive(!step.useAdditionalOverlay);
        
        if (additionalOverlay != null)
            additionalOverlay.SetActive(step.useAdditionalOverlay);

        
        foreach (var t in step.texts)
        {
            if (t.textUI != null)
                t.textUI.text = t.message;
        }

       
        if (currentHighlightGO != null)
        {
            Destroy(currentHighlightGO);
        }

        if (step.targetButton != null)
        {
            GameObject parentOverlay = step.useAdditionalOverlay && additionalOverlay != null ? 
                additionalOverlay : tutorialOverlay;

           
            currentHighlightGO = new GameObject("TutorialHighlight", typeof(RectTransform), typeof(Image), typeof(Mask));
            currentHighlightGO.transform.SetParent(parentOverlay.transform, false);

            RectTransform highlightRect = currentHighlightGO.GetComponent<RectTransform>();
            RectTransform buttonRect = step.targetButton.GetComponent<RectTransform>();

            highlightRect.anchoredPosition = buttonRect.anchoredPosition;
            highlightRect.sizeDelta = buttonRect.sizeDelta;
            highlightRect.anchorMin = buttonRect.anchorMin;
            highlightRect.anchorMax = buttonRect.anchorMax;
            highlightRect.pivot = buttonRect.pivot;
            highlightRect.localScale = buttonRect.localScale;

            Mask mask = currentHighlightGO.GetComponent<Mask>();
            mask.showMaskGraphic = false;

            Image highlightImage = currentHighlightGO.GetComponent<Image>();
            highlightImage.color = new Color(1f, 1f, 1f, 0f);
            highlightImage.raycastTarget = false;

            step.targetButton.onClick.RemoveListener(OnTargetButtonClicked);
            step.targetButton.onClick.AddListener(OnTargetButtonClicked);

            
            if (pulseCoroutine != null)
            {
                StopCoroutine(pulseCoroutine);
            }
            pulseCoroutine = StartCoroutine(PulseEffect(step.targetButton.transform));
        }
    }

    IEnumerator PulseEffect(Transform target)
    {
        if (target == null) yield break;

        Vector3 originalScale = target.localScale;
        float pulseSpeed = 2f;

        while (currentHighlightGO != null && target != null)
        {
            float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.1f;
            target.localScale = originalScale * scale;
            yield return null;
        }

        if (target != null)
            target.localScale = originalScale;
    }

    void OnTargetButtonClicked()
    {
        StopPulseEffect();

        if (currentStepIndex >= 0 && currentStepIndex < tutorialSteps.Count)
        {
            if (tutorialSteps[currentStepIndex].targetButton != null)
            {
                tutorialSteps[currentStepIndex].targetButton.transform.localScale = Vector3.one;
                tutorialSteps[currentStepIndex].targetButton.onClick.RemoveListener(OnTargetButtonClicked);
            }
        }

        NextStep();
    }

    void StopPulseEffect()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }
    }

    void EndTutorial()
    {
        Debug.Log("Ending Tutorial - Starting Cleanup...");
        
        StopPulseEffect();
        StopAllCoroutines();
        
        
        ClearTargetButtonListener();
        
        
        ClearAllStepListeners();
        
        
        RestoreAllButtons();
        
        
        ClearAllTutorialTexts();
        
        
        if (tutorialOverlay != null)
            tutorialOverlay.SetActive(false);
        
        if (additionalOverlay != null)
            additionalOverlay.SetActive(false);

        
        if (currentHighlightGO != null)
        {
            Destroy(currentHighlightGO);
            currentHighlightGO = null;
        }

       
        ResetShopToInitialState();

        Debug.Log("Shop Tutorial Completed and Reset!");
    }

    void ClearAllStepListeners()
    {
        foreach (var step in tutorialSteps)
        {
            if (step.targetButton != null)
            {
                step.targetButton.onClick.RemoveListener(OnTargetButtonClicked);
            }
        }
    }

    void ResetShopToInitialState()
    {
        Debug.Log("Resetting Shop to Initial State...");
        
        // Reset tutorial variables
        currentStepIndex = -1;
        
        
        if (currentHighlightGO != null)
        {
            Destroy(currentHighlightGO);
            currentHighlightGO = null;
        }
        
        
        originalButtonStates.Clear();
        
        
        foreach (Button button in allButtons)
        {
            if (button != null)
            {
                
                button.onClick.RemoveListener(OnTargetButtonClicked);
                
                
                button.interactable = true;
                button.gameObject.SetActive(true);
                
                
                button.transform.localScale = Vector3.one;
            }
        }
        
        
        foreach (var textUI in allTextUIs)
        {
            if (textUI != null)
            {
                textUI.text = "";
            }
        }
        
        
        
        
        Debug.Log("Shop Reset Complete!");
    }

    

    public void StopTutorial()
    {
        Debug.Log("Tutorial Stopped by User");
        EndTutorial();
    }

    
    public void RestartTutorial()
    {
        Debug.Log("Restarting Tutorial...");
        StopTutorial();
        StartTutorial();
    }

    
    public bool IsTutorialActive()
    {
        return currentStepIndex >= 0 && currentStepIndex < tutorialSteps.Count;
    }

    void OnDestroy()
    {
        
        StopAllCoroutines();
        ClearAllStepListeners();
        
        if (currentHighlightGO != null)
        {
            Destroy(currentHighlightGO);
        }
    }
}