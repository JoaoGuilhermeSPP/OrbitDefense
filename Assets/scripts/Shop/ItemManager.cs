using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [Header("Visual da Nave")]
    public SpriteRenderer Skin;
    public Animator skinAnimator;

    [Header("Todas as skins")]
    public SkinShop[] Itens;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(
       SaveManager.instance.data.equipedItem))
        {
            EquipIten(Itens[0]); // skin padrão
        }
        else
        {
            LoadEquippedIten();
        }
    }

    public void EquipIten(SkinShop item)
    {
        if (item == null)
            return;

        // Sprite da nave
        Skin.sprite = item.skin;

        // Animator da nave
        skinAnimator.runtimeAnimatorController =
            item.controller;

        // Sprite da bala
        PlayerMove.instance.currentBulletSprite =
            item.BulletSkin;

        // Salvar item equipado
        SaveManager.instance.data.equipedItem =
            item.ItemId;

        SaveManager.instance.Save();
    }

    public void LoadEquippedIten()
    {
        string equipped =
            SaveManager.instance.data.equipedItem;

        foreach (var item in Itens)
        {
            if (item.ItemId == equipped)
            {
                EquipIten(item);
                break;
            }
        }
    }
}