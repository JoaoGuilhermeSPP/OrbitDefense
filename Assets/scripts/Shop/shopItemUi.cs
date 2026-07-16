using TMPro;
using UnityEngine;

public class shopItemUi : MonoBehaviour
{
    public SkinShop item;
    public TextMeshProUGUI buttonTxt;

    public void UpdateUI()
    {
        bool unlocked = SaveManager.instance.data.itemsUnlock.Contains(item.ItemId);


        if (!unlocked)
        {
            buttonTxt.text = item.preco.ToString();

            return;
        }
        if (SaveManager.instance.data.equipedItem == item.ItemId)
        {
            buttonTxt.text = "Equipped";
        }
        else
        {
            buttonTxt.text = "Equip";
        }
    }
    public void OnButtonClick()
    {
        bool unlocked = SaveManager.instance.data.itemsUnlock.Contains(item.ItemId);

        if (!unlocked)
        {
            Buy();
        }
        else
        {
            Equip();
        }
        void Buy()
        {
            int coins = SaveManager.instance.data.totalCoins;
            if (coins < item.preco)
            {
                return;
            }
            SaveManager.instance.RemoveCoins(item.preco);

            SaveManager.instance.data.itemsUnlock.Add(item.ItemId);

            SaveManager.instance.Save();

            ShopManager.instance.RefreshAllItems();
        }
        void Equip()
        {
            SaveManager.instance.data.equipedItem =
                item.ItemId;

            SaveManager.instance.Save();

            ShopManager.instance.RefreshAllItems();
        }
    }
}
