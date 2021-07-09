using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolSystem : MonoBehaviour
{
    [SerializeField] protected ScrollRect poolScrollView;
    [SerializeField] protected RectTransform prefabsCell;

    protected List<RectTransform> poolCells;
    protected List<ItemCell> itemCells;
    protected int totalItemData;

    protected Vector2 cellSize;
    [SerializeField] protected Vector2 distanceCell;

    private void Start() {
        poolScrollView.onValueChanged.AddListener(UpdatePool);
    }

    public virtual void Init() {}
    protected virtual void CreatPool() {}
    protected virtual void UpdatePool(Vector2 direction) {}
}
