using UnityEngine;

public class PopupUI : BaseUI
{
    public void ShowPopUp()
    {
        ServiceLocator.Get<UIManager>().Add(this);
    }
    public void HidePopup()
    {
        ServiceLocator.Get<UIManager>().Remove(this);
        Destroy(gameObject);
    }
}
