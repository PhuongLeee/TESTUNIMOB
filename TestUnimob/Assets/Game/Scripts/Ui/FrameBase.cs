using UnityEngine;
using UnityEngine.UI;

public class FrameBase : MonoBehaviour
{
    [SerializeField] protected Button btnClose;

    protected virtual void Start() {
        if(btnClose!=null) btnClose.onClick.AddListener(OnButtonClose);
    }

    public virtual void Show() {
        gameObject.SetActive(true);
    }
    public virtual void Hide() {
        gameObject.SetActive(false);
    }

    protected virtual void OnButtonClose() {
        Hide();
    }

}
