using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using GameCommon;
//using SimpleJSON;

public class ReadJson : SingletonBind<ReadJson>
{
    string fileJSONSave = "";
    [SerializeField] string jsonTxt;
    [SerializeField] TextAsset jsonTextAsset;

    [HideInInspector] public List<Item> listShopItems = new List<Item>();

    private void Awake()
    {
        jsonTxt = jsonTextAsset.ToString();
    }


    public void ReadJsonMethod()
    {
        jsonTxt = jsonTextAsset.ToString();
        print(jsonTxt);
        JsonData json = JsonMapper.ToObject(jsonTxt);
        int numShopItem = json["items"].Count;
        print(numShopItem);
        for (int i = 0; i < numShopItem; i++)
        {
            int index = i;
            Item shopee = new Item();
            shopee.id = (int)json["items"][i]["id"];
            shopee.icon = json["items"][i]["icon"].ToString();
            shopee.title = json["items"][i]["title"].ToString();
            shopee.desc = json["items"][i]["desc"].ToString();
            shopee.price = (int)json["items"][i]["price"];
            listShopItems.Add(shopee);
        }
    }
}
