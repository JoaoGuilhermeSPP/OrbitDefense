using UnityEngine;

public class ShopManager : MonoBehaviour
{
   public static ShopManager instance;
    public shopItemUi[] items;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        RefreshAllItems();
    }
    public void RefreshAllItems()
    {
        foreach(var item in items)
        {
            item.UpdateUI();
        }   
    }
}
