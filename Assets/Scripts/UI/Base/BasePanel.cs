using UnityEngine;

namespace ARPG.UI
{
    public class BasePanel : MonoBehaviour
    {
        protected bool isRemove = false;

        protected new string name;

        public virtual void OpenPanel(string name)
        {
            this.name = name;
            gameObject.SetActive(true);
        }

        public virtual void ClosePanel()
        {
            isRemove = true;
            gameObject.SetActive(false);
            Destroy(gameObject);

            if (UIManager.Instance.panelDict.ContainsKey(name))
            {
                UIManager.Instance.panelDict.Remove(name);
            }
        }
    }
}