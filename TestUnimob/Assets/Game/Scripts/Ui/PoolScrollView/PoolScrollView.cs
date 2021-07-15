using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolScrollView : ScrollRect
{
    public void SetContentStart(Vector2 vt) {
        m_ContentStartPosition += vt;
    }
}
