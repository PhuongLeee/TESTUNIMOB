using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
    public int indexData;
    public virtual void Init(Item itemShop) {}
    public virtual void SetRow(int row) { }
    public virtual void SetCollumn(int collumn) { }
}
