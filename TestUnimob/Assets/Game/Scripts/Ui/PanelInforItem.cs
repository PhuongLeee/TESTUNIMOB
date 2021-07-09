using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelInforItem : FrameBase
{
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtDes;
    [SerializeField] private Image imgDes;
    [SerializeField] private Animator anim;
    private string animKey = "DetailEnd";

    public void Show(int idItem) {
        var data = ReadJson.Instance.listShopItems[idItem];
        txtName.text = data.title.ToString();
        txtDes.text = data.desc.ToString();
        imgDes.sprite = UiController.Instance.GetSpriteItemShop(data.icon);
        Show();
    }

    protected override void OnButtonClose() {
      //  base.OnButtonClose();
        anim.Play(animKey);
    }
}
