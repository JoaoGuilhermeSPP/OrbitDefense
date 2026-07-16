using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class SkinShop : ScriptableObject
{
    public string ItemId;
    public int preco;
    public Sprite skin;
    public Sprite BulletSkin;
    public RuntimeAnimatorController controller;
}


