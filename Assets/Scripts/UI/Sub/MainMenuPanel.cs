using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARPG.UI
{
    public class MainMenuPanel : BasePanel
    {
        public GameObject StartTimeline;

        public void Init()
        {
        }

        public void StartGameButton()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartTimeline = GameObject.Find("Director");
            GameObject.Find("CAMERAS").SetActive(false);
            StartTimeline.SetActive(false);
            UIManager.Instance.OpenPanel(UIConst.PlayerStatePanel);
            UIManager.Instance.ClosePanel(UIConst.MainMenuPanel);
        }

        public void SettingButton()
        {
            UIManager.Instance.OpenPanel("SettingPanel");
        }

        public void ExitGameButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}