using System;
using UnityEngine;

namespace ARPG.Dialogue
{
    public enum DialogueHuman
    {
        Left, Right
    }

    [Serializable]
    public class DialogueSetting
    {
        public string name;
        public string textContent;
        public DialogueHuman human;
    }

    public class DialogueSettings
    {
        public Sprite LeftPic;
        public Sprite RightPic;
        public DialogueSetting[] content;

        public DialogueSettings(Sprite leftPic, Sprite rightPic, DialogueSetting[] content)
        {
            LeftPic = leftPic;
            RightPic = rightPic;
            this.content = content;
        }
    }

    public class InteractableBase : MonoBehaviour
    {
        public void OnTriggerByPlayer()
        {
            Interact();
        }

        protected virtual void Interact()
        {
        }
    }
}