using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCommon;
public class ShopController : FrameBase
{
    [SerializeField] private VerticlePoolSystem scrolRect;

    protected override void  Start() {
        base.Start();
        scrolRect.itemDatas = ReadJson.Instance.listShopItems;
        scrolRect.Init();
    }

    public override void Show() {
        base.Show();
    }
}
