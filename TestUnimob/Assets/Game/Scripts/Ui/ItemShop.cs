using UnityEngine;
using UnityEngine.UI;

public class ItemShop : ItemCell
{
    [Header("ItemShop")]
    [SerializeField] private Image imgItem;
    [SerializeField] private Image imgRowItem;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtPrice;

    public override void Init(Item itemShop) {
        base.Init(itemShop);
        txtName.text = itemShop.title;
        txtPrice.text = itemShop.price.ToString();
        imgItem.sprite = UiController.Instance.GetSpriteItemShop(itemShop.icon);
    }

    public override void SetRow(int row) {
        base.SetRow(row);
        if(row == 0) {
            imgRowItem.gameObject.SetActive(true);
        } else {
            imgRowItem.gameObject.SetActive(false);
        }
    }

    public void OnButtonShowDetail() {
        UiController.Instance.panelInfor.Show(indexData);
    }
}
