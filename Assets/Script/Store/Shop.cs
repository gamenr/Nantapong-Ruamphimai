using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public int price;
    public Image itemImage;

    [Header("Auto Price Display")]
    public TextMeshProUGUI priceText; 
    public GameObject ownedIcon; 
    [HideInInspector] public bool isOwned = false; 
}

public class Shop : MonoBehaviour
{
    public int Coin = 4000;
    public TextMeshProUGUI Coine_text;

    [Header("Main Shop Buttons")]
    public Button buyButton;
    public Button sellButton;

    [Header("Item Lists")]
    public List<ShopItem> items;
    public List<ShopItem> specialItems;

    public Button clearSaveButton;

    private int selectedIndex = -1;
    private int currentListIndex = 0;

    void Start()
    {
        LoadGameData();
        Coine_text.text = Coin.ToString();
        
        
        UpdateAllPriceDisplays();
        
        
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(BuySelectedItem);
        }
        
        if (sellButton != null)
        {
            sellButton.onClick.AddListener(SellSelectedItem);
        }
        
       
        UpdateBuySellButtonsUI();

        if (clearSaveButton != null)
        {
            clearSaveButton.onClick.AddListener(ClearSave);
        }
    }

    void LoadGameData()
    {
        Coin = PlayerPrefs.GetInt("PlayerCoins", 4000);
        currentListIndex = PlayerPrefs.GetInt("CurrentListIndex", 0);
        
       
        foreach (var item in items)
        {
            item.isOwned = HasPurchasedItem(item.itemName);
        }
        foreach (var item in specialItems)
        {
            item.isOwned = HasPurchasedItem(item.itemName);
        }
        
        
        UpdateAllItemUIs();
        
        Debug.Log("Game Loaded - Coins: " + Coin);
    }

    public void UpdateAllPriceDisplays()
    {
        for (int i = 0; i < items.Count; i++)
            UpdateItemPriceDisplay(items[i]);

        for (int i = 0; i < specialItems.Count; i++)
            UpdateItemPriceDisplay(specialItems[i]);

        Debug.Log("All price displays updated");
    }

    private void UpdateItemPriceDisplay(ShopItem item)
    {
        if (item.priceText != null)
            item.priceText.text = item.price.ToString();
    }

    public void UpdateCurrentListPriceDisplays()
    {
        List<ShopItem> currentItems = GetCurrentItemList();
        for (int i = 0; i < currentItems.Count; i++)
            UpdateItemPriceDisplay(currentItems[i]);

        string listName = currentListIndex == 0 ? "Normal Items" : "Special Items";
        Debug.Log("Updated price displays for " + listName);
    }

    public void SwitchItemList(int listIndex)
    {
        if (listIndex < 0 || listIndex > 1) return;
        currentListIndex = listIndex;
        selectedIndex = -1;
        UpdateCurrentListPriceDisplays();
        SaveCurrentState();
        UpdateAllItemUIs(); 
        UpdateBuySellButtonsUI(); 
        string listName = currentListIndex == 0 ? "Normal Items" : "Special Items";
        Debug.Log("Switched to " + listName);
    }

    public void SetItemPrice(int itemIndex, int newPrice, bool isSpecialItem = false)
    {
        List<ShopItem> targetList = isSpecialItem ? specialItems : items;
        if (itemIndex < 0 || itemIndex >= targetList.Count) return;
        targetList[itemIndex].price = newPrice;
        UpdateItemPriceDisplay(targetList[itemIndex]);
        Debug.Log("Updated " + targetList[itemIndex].itemName + " price to " + newPrice);
    }

    private void SaveCurrentState()
    {
        PlayerPrefs.SetInt("CurrentListIndex", currentListIndex);
        PlayerPrefs.Save();
    }

    private List<ShopItem> GetCurrentItemList()
    {
        return currentListIndex == 0 ? items : specialItems;
    }

    public void SelectItem(int index)
    {
        List<ShopItem> currentItems = GetCurrentItemList();
        if (index < 0 || index >= currentItems.Count)
        {
            selectedIndex = -1;
            UpdateBuySellButtonsUI();
            return;
        }

        selectedIndex = index;
        Debug.Log("Selected " + currentItems[index].itemName + " price " + currentItems[index].price + " from " + (currentListIndex == 0 ? "Normal" : "Special") + " list");
        
        UpdateBuySellButtonsUI();
    }

    public void BuySelectedItem()
    {
        if (selectedIndex < 0)
        {
            Debug.Log("No item selected");
            return;
        }

        List<ShopItem> currentItems = GetCurrentItemList();
        ShopItem selectedItem = currentItems[selectedIndex];

        if (selectedItem.isOwned)
        {
            Debug.Log("Already owned " + selectedItem.itemName);
            return;
        }

        if (Coin >= selectedItem.price)
        {
            Coin -= selectedItem.price;
            Coine_text.text = Coin.ToString();

            SavePurchasedItem(selectedItem.itemName);
            selectedItem.isOwned = true; 
            SaveCoins();

            UpdateItemUI(selectedItem);
            UpdateBuySellButtonsUI();

            Debug.Log("Bought " + selectedItem.itemName + " for " + selectedItem.price + " coins");
        }
        else
        {
            Debug.Log("Not enough Coins");
        }
    }

    public void SellSelectedItem()
    {
        if (selectedIndex < 0)
        {
            Debug.Log("No item selected");
            return;
        }

        List<ShopItem> currentItems = GetCurrentItemList();
        ShopItem selectedItem = currentItems[selectedIndex];

        if (selectedItem.isOwned)
        {
            RemovePurchasedItem(selectedItem.itemName);
            selectedItem.isOwned = false; 
            
            Coin += selectedItem.price;
            Coine_text.text = Coin.ToString();
            SaveCoins();

            UpdateItemUI(selectedItem);
            UpdateBuySellButtonsUI();

            Debug.Log("Sold " + selectedItem.itemName + " for " + selectedItem.price + " coins");
        }
        else
        {
            Debug.Log("You don't own " + selectedItem.itemName);
        }
    }

    public bool IsCurrentItemOwned(int index)
    {
        List<ShopItem> currentItems = GetCurrentItemList();
        if (index < 0 || index >= currentItems.Count) return false;
        return HasPurchasedItem(currentItems[index].itemName);
    }

    public int GetCurrentItemCount()
    {
        List<ShopItem> currentItems = GetCurrentItemList();
        return currentItems != null ? currentItems.Count : 0;
    }

    public ShopItem GetCurrentItem(int index)
    {
        List<ShopItem> currentItems = GetCurrentItemList();
        if (index < 0 || index >= currentItems.Count) return null;
        return currentItems[index];
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("PlayerCoins", Coin);
        PlayerPrefs.Save();
    }

    private void SavePurchasedItem(string itemName)
    {
        PlayerPrefs.SetInt(itemName + "_owned", 1);
        PlayerPrefs.Save();
    }

    private bool HasPurchasedItem(string itemName)
    {
        return PlayerPrefs.GetInt(itemName + "_owned", 0) == 1;
    }

    private void RemovePurchasedItem(string itemName)
    {
        PlayerPrefs.DeleteKey(itemName + "_owned");
        PlayerPrefs.Save();
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
        Coin = 4000;
        currentListIndex = 0;
        selectedIndex = -1;
        Coine_text.text = Coin.ToString();
        
        foreach (var item in items)
        {
            item.isOwned = false;
        }
        foreach (var item in specialItems)
        {
            item.isOwned = false;
        }

        UpdateAllPriceDisplays();
        UpdateAllItemUIs();
        UpdateBuySellButtonsUI();
        Debug.Log("All save data cleared");
    }

    public bool IsItemOwned(int index)
    {
        if (index < 0 || index >= items.Count) return false;
        return HasPurchasedItem(items[index].itemName);
    }

    public bool IsSpecialItemOwned(int index)
    {
        if (index < 0 || index >= specialItems.Count) return false;
        return HasPurchasedItem(specialItems[index].itemName);
    }

    public string GetCurrentListName()
    {
        return currentListIndex == 0 ? "Normal Items" : "Special Items";
    }

    public int GetCurrentListIndex()
    {
        return currentListIndex;
    }

    public bool IsShowingSpecialItems()
    {
        return currentListIndex == 1;
    }
    
    private void UpdateItemUI(ShopItem item)
    {
        bool owned = item.isOwned;

        if (item.ownedIcon != null)
            item.ownedIcon.SetActive(owned); 
    }

    private void UpdateAllItemUIs()
    {
        foreach (var item in items)
            UpdateItemUI(item);

        foreach (var item in specialItems)
            UpdateItemUI(item);
    }
    
    private void UpdateBuySellButtonsUI()
    {
        bool itemIsOwned = false;
        
        if (selectedIndex >= 0)
        {
            List<ShopItem> currentItems = GetCurrentItemList();
            if (selectedIndex < currentItems.Count)
            {
                itemIsOwned = currentItems[selectedIndex].isOwned;
            }
        }
        
        if (buyButton != null)
        {
            buyButton.interactable = (selectedIndex >= 0 && !itemIsOwned);
        }
        
        if (sellButton != null)
        {
            sellButton.interactable = (selectedIndex >= 0 && itemIsOwned);
        }
    }
}