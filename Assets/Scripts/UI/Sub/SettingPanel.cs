using UnityEngine.UI;

namespace ARPG.UI
{
    public class SettingPanel : BasePanel
    {
        public void ExitButton()
        {
            UIManager.Instance.ClosePanel("SettingPanel");
        }
    }
}