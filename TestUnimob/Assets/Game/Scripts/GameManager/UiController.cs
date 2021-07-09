using UnityEngine;
using GameCommon;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UiController : SingletonBind<UiController>
{
    [SerializeField] private ShopController shopController;
    public PanelInforItem panelInfor;

    [SerializeField] private SpriteAtlas shopItemSprites;
    protected override void Awake() {
        base.Awake();
        ReadJson.Instance.ReadJsonMethod();
        shopController.Show();
    }

    public Sprite GetSpriteItemShop(string name) {
        return shopItemSprites.GetSprite(name);
    }
}
