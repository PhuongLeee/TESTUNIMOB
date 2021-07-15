using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticlePoolSystem : PoolSystem
{
    [Header("Verticle Pool System")]
    public int numColunm;
    public float minRatioCoverage;
    private int maxTopCellIndex;
    private int maxBottomCellIndex;
    private int currentDataIndex;
    private Vector3 posCell;
    private bool isUpdating;
    protected int startIndex;
    protected int endIndex;
    private Bounds poolViewBounds;
    private readonly Vector3[] corners = new Vector3[4];
    protected float RecyclingThreshold = .2f;
    [HideInInspector] public List<Item> itemDatas;
    private Vector2 prevAnchoredPos;

    public override void Init() {
        base.Init();
        CreatPool();
        InitScrollView();
        SetRecyclingBounds();
    }

    protected override void CreatPool() {        
        if (poolCells != null) {
            poolCells.ForEach((RectTransform item) => UnityEngine.Object.Destroy(item.gameObject));
            poolCells.Clear();
        } else {
            poolCells = new List<RectTransform>();
        }
        if (itemCells != null) itemCells.Clear();
        else itemCells = new List<ItemCell>();
        totalItemData = itemDatas.Count;
        poolViewBounds = new Bounds();
        prevAnchoredPos = poolScrollView.content.anchoredPosition;
    }

    private void InitScrollView() {
        startIndex = endIndex = 0;
        maxTopCellIndex = 0;
        float currentPoolCoverage = 0;
        posCell = Vector3.zero;

        cellSize.x = (poolScrollView.content.rect.width - distanceCell.x*(numColunm-1)) / numColunm;
        cellSize.y = prefabsCell.sizeDelta.y / prefabsCell.sizeDelta.x * cellSize.x;
        posCell.x = cellSize.x / 2;
        posCell.y = -cellSize.y / 2;
        float requriedCoverage = 1.5f * poolScrollView.viewport.rect.height;
        
        RectTransform cell;
        while(currentPoolCoverage < requriedCoverage && poolCells.Count < totalItemData) {
            cell = Instantiate<RectTransform>(prefabsCell);
            cell.SetParent(poolScrollView.content);
            cell.localScale = Vector3.one;
            cell.sizeDelta = cellSize;
            posCell.x = endIndex * cellSize.x + cellSize.x / 2 + endIndex * distanceCell.x;      
            cell.anchoredPosition = posCell;

            poolCells.Add(cell);
            itemCells.Add(cell.GetComponent<ItemCell>());
            itemCells[itemCells.Count - 1].indexData = itemCells.Count-1;
            itemCells[itemCells.Count - 1].Init(itemDatas[itemCells.Count - 1]);
            itemCells[itemCells.Count - 1].SetRow(endIndex);
            currentDataIndex++;
            endIndex++;
            if (endIndex >= numColunm) {
                endIndex = 0;
                posCell.y -= cellSize.y + distanceCell.y;
                currentPoolCoverage += cell.rect.height;
            }
        }
        endIndex--;
        if (endIndex < 0) endIndex = numColunm-1;
        int numRows = (int)Mathf.Ceil((float)poolCells.Count / (float)numColunm);
        poolScrollView.content.sizeDelta = new Vector2(poolScrollView.content.sizeDelta.x, numRows * cellSize.y + (numRows - 1) * distanceCell.y);
        maxBottomCellIndex = poolCells.Count - 1;
    }

    protected override void UpdatePool(Vector2 direction) {        
        base.UpdatePool(direction);
        if (poolCells == null || poolCells.Count == 0 || isUpdating) return;
        SetRecyclingBounds();

        direction = poolScrollView.content.anchoredPosition - prevAnchoredPos;
        prevAnchoredPos = poolScrollView.content.anchoredPosition;

        if (direction.y > 0 && poolCells[maxBottomCellIndex].MaxY() > poolViewBounds.min.y) {
                PoolTopToBottomDir();
        } else if (direction.y < 0 && poolCells[maxTopCellIndex].MinY() < poolViewBounds.max.y) {
            PoolBottomToTopDir();
        }
    }
    
    private void PoolTopToBottomDir() {
        isUpdating = true;
        int numRowAdd = 0;
        int numRow = 0;
        int dem = 0;
        while (poolCells[maxTopCellIndex].MinY() > poolViewBounds.max.y && currentDataIndex < totalItemData) {
            dem++;
            endIndex++;
            startIndex++;
            if (endIndex >= numColunm) {
                endIndex = 0;
                numRowAdd++;
                numRow++;
            }
            if(startIndex >= numColunm) {
                startIndex = 0;
                numRow--;
            }
            SetPosstionNextBottom(maxTopCellIndex);
           
            itemCells[maxTopCellIndex].indexData = currentDataIndex;
            itemCells[maxTopCellIndex].Init(itemDatas[currentDataIndex]);
            itemCells[maxTopCellIndex].SetRow(endIndex);
            maxTopCellIndex++;
            currentDataIndex++;
            if (maxTopCellIndex >= poolCells.Count) {
                maxTopCellIndex = 0;
            }
        }
        if (numRow > 0) numRowAdd -= numRow;
        poolScrollView.content.sizeDelta += Vector2.up * numRow * (distanceCell.y + cellSize.y);
        if (numRowAdd > 0) {
            poolScrollView.content.anchoredPosition -= Vector2.up * (numRowAdd * cellSize.y + numRowAdd * distanceCell.y);
            UpdatePossitionCellPool(numRowAdd * cellSize.y + numRowAdd * distanceCell.y);
            poolScrollView.SetContentStart(Vector2.down * (numRowAdd * cellSize.y + numRowAdd * distanceCell.y));
        }
        isUpdating = false;
    }

    private void PoolBottomToTopDir() {
        isUpdating = true;
        int numRowAdd = 0;
        int numRow = 0;
        while (poolCells[maxBottomCellIndex].MaxY() < poolViewBounds.min.y && currentDataIndex - poolCells.Count > 0) {
            endIndex--;
            startIndex--;
            if (endIndex < 0) {
                endIndex = numColunm-1;
                  numRow--;
            }
            if (startIndex < 0) {
                startIndex = numColunm-1;
                numRowAdd++;
                numRow++;
            }
            SetPosstionNextTop(maxBottomCellIndex);            
            currentDataIndex--;

            itemCells[maxBottomCellIndex].indexData = currentDataIndex - poolCells.Count;
            itemCells[maxBottomCellIndex].Init(itemDatas[currentDataIndex - poolCells.Count]);
            itemCells[maxBottomCellIndex].SetRow(startIndex);

            maxBottomCellIndex--;
            if (maxBottomCellIndex < 0) {
                maxBottomCellIndex = poolCells.Count - 1;
            }
        }
        if (numRow > 0) numRowAdd -= numRow;
        poolScrollView.content.sizeDelta += Vector2.up * numRow * (distanceCell.y + cellSize.y);
        if (numRowAdd > 0) {
            UpdatePossitionCellPool(-numRowAdd * cellSize.y - numRowAdd * distanceCell.y);
            poolScrollView.content.anchoredPosition += Vector2.up * (numRowAdd * cellSize.y + numRowAdd * distanceCell.y);

            poolScrollView.SetContentStart(Vector2.up * (numRowAdd * cellSize.y + numRowAdd * distanceCell.y));
        }
        isUpdating = false;
    }

    private void SetPosstionNextBottom(int indexCell) {
        var cellBottomPos = poolCells[maxBottomCellIndex].anchoredPosition;
        if (endIndex == 0) {
            posCell.y = cellBottomPos.y - distanceCell.y - cellSize.y;
            posCell.x = cellSize.x / 2;
        } else {
            posCell.y = cellBottomPos.y;
            posCell.x = cellBottomPos.x + distanceCell.x + cellSize.x;
        }
        maxBottomCellIndex = indexCell;
        poolCells[indexCell].anchoredPosition = posCell;
      //  cell.anchoredPosition = poolCells[maxBottomCellIndex].anchoredPosition+
    }

    private void SetPosstionNextTop(int indexCell) {
        var cellBottomPos = poolCells[maxTopCellIndex].anchoredPosition;
        if (startIndex == numColunm-1) {
            posCell.y = cellBottomPos.y + distanceCell.y + cellSize.y;
            posCell.x = cellSize.x * (startIndex+ .5f) + startIndex * distanceCell.x;
        } else {
            posCell.y = cellBottomPos.y;
            posCell.x = cellBottomPos.x - distanceCell.x - cellSize.x;
        }
        maxTopCellIndex = indexCell;
        poolCells[indexCell].anchoredPosition = posCell;
        //  cell.anchoredPosition = poolCells[maxBottomCellIndex].anchoredPosition+
    }

    private void UpdatePossitionCellPool(float yDis) {
        for(int i = 0; i < poolCells.Count; i++) {
            poolCells[i].anchoredPosition += Vector2.up * yDis;
        }
    }
    private void SetRecyclingBounds() {
        poolScrollView.viewport.GetWorldCorners(corners);
        float threshHold = RecyclingThreshold * (corners[2].y - corners[0].y);
        poolViewBounds.min = new Vector3(corners[0].x, corners[0].y - threshHold);
        poolViewBounds.max = new Vector3(corners[2].x, corners[2].y + threshHold);
    }
}
