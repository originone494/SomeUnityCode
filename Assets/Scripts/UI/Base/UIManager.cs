using System.Collections.Generic;
using UnityEngine;

namespace ARPG.UI
{
    public class UIConst
    {
        public const string MainMenuPanel = "MainMenuPanel";
        public const string SettingPanel = "SettingPanel";
        public const string PlayerStatePanel = "PlayerStateBar";
        public const string EnemyStatePanel = "EnemyStateBar";
        public const string DialoguePanel = "DialoguePanel";
    }

    public class UIManager
    {
        private static UIManager _instance;
        private Transform _uiRoot;
        private Dictionary<string, string> pathDict;
        private Dictionary<string, GameObject> prefabsDict;
        public Dictionary<string, BasePanel> panelDict;

        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIManager();
                }
                return _instance;
            }
        }

        public Transform UIRoot
        {
            get
            {
                if (_uiRoot == null)
                {
                    if (GameObject.Find("Canvas"))
                    {
                        _uiRoot = GameObject.Find("Canvas").transform;
                    }
                    else
                    {
                        _uiRoot = new GameObject("Canvas").transform;
                    }
                }
                return _uiRoot;
            }
        }

        private UIManager()
        {
            InitDicts();
        }

        private void InitDicts()
        {
            prefabsDict = new Dictionary<string, GameObject>();
            panelDict = new Dictionary<string, BasePanel>();
            pathDict = new Dictionary<string, string>()
            {
                { UIConst.MainMenuPanel ,UIConst.MainMenuPanel},
                { UIConst.SettingPanel ,UIConst.SettingPanel},
                { UIConst.PlayerStatePanel ,UIConst.PlayerStatePanel},
                { UIConst.EnemyStatePanel ,UIConst.EnemyStatePanel},
                { UIConst.DialoguePanel ,UIConst.DialoguePanel}
            };
        }

        public BasePanel OpenPanel(string name)
        {
            BasePanel panel = null;
            if (panelDict.TryGetValue(name, out panel))
            {
                Debug.Log("界面已打开");
                return null;
            }

            string path = "";
            if (!pathDict.TryGetValue(name, out path))
            {
                Debug.Log("界面名称错误，或未配置路径 " + name);
                return null;
            }
            GameObject panelPrefab = null;
            if (!prefabsDict.TryGetValue(name, out panelPrefab))
            {
                string realPath = "Prefabs/UI/" + path;
                panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
                prefabsDict.Add(name, panelPrefab);
            }

            GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
            panelObject.transform.position = UIRoot.position;
            panel = panelObject.GetComponent<BasePanel>();
            panelDict.Add(name, panel);
            panel.OpenPanel(name);
            return panel;
        }

        public bool ClosePanel(string name)
        {
            BasePanel panel = null;
            if (!panelDict.TryGetValue(name, out panel))
            {
                Debug.Log("界面未打开" + name);
                return false;
            }

            panel.ClosePanel();
            return true;
        }
    }
}