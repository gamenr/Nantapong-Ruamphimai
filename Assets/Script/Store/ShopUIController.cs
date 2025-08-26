using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [Header("Shop Buttons")]
    public Button item1Button;
    public Button item2Button;
    

    [Header("Shop GameObjects")]
    public GameObject item1GameObject;
    public GameObject item2GameObject;

    [Header("Shop Reference")]
    public Shop shopScript;

    void Start()
    {
        ShowOnlyItem1();
        
        if (item1Button != null)
        {
            item1Button.onClick.AddListener(() => SwitchToItem1());
        }
        
        if (item2Button != null)
        {
            item2Button.onClick.AddListener(() => SwitchToItem2());
        }
    }

    public void SwitchToItem1()
    {
        if (shopScript != null)
        {
            shopScript.SwitchItemList(0);
        }
        ShowOnlyItem1();
    }

    public void SwitchToItem2()
    {
        if (shopScript != null)
        {
            shopScript.SwitchItemList(1);
        }
        ShowOnlyItem2();
    }

    private void ShowOnlyItem1()
    {
        if (item1GameObject != null)
            item1GameObject.SetActive(true);
            
        if (item2GameObject != null)
            item2GameObject.SetActive(false);
    }

    private void ShowOnlyItem2()
    {
        if (item1GameObject != null)
            item1GameObject.SetActive(false);
            
        if (item2GameObject != null)
            item2GameObject.SetActive(true);
    }

    public void UpdateUIBasedOnCurrentList()
    {
        if (shopScript == null) return;
        
        int currentList = shopScript.GetCurrentListIndex();
        
        if (currentList == 0)
        {
            ShowOnlyItem1();
        }
        else if (currentList == 1)
        {
            ShowOnlyItem2();
        }
    }
}