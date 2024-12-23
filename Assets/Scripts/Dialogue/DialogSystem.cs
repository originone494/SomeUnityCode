using ARPG.UI;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ARPG.Dialogue
{
    public class DialogSystem : MonoBehaviour
    {
        public static DialogSystem Instance;
        public float textSpeed;

        private float RealTextSpeed;
        private DialogueSettings currentDialogue;
        private DialoguePanel panel;
        private int length;
        private bool isDialogue;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject); return;
            }
            Instance = this;
            isDialogue = false;
        }

        private void Update()
        {
            if (isDialogue && Input.GetKeyDown(KeyCode.F))
            {
                next();
            }
        }

        public void interact(DialogueSettings dialogue, BasePanel basePanel)
        {
            GameManager.Instance.blackBoard.SetValue(GameManager.Instance.isInteractString, true);
            UIManager.Instance.ClosePanel(UIConst.PlayerStatePanel);
            length = 0;
            currentDialogue = dialogue;
            panel = basePanel.gameObject.GetComponent<DialoguePanel>();
            panel.Init(currentDialogue.LeftPic, currentDialogue.RightPic);
            isDialogue = true;
        }

        public void next()
        {
            Debug.Log(length);
            if (length == currentDialogue.content.Length)
            {
                UIManager.Instance.ClosePanel(UIConst.DialoguePanel);
                UIManager.Instance.OpenPanel(UIConst.PlayerStatePanel);
                GameManager.Instance.blackBoard.SetValue(GameManager.Instance.isInteractString, false);
                isDialogue = false;
                return;
            }
            panel.display(currentDialogue.content[length].name, currentDialogue.content[length].textContent, currentDialogue.content[length].human);
            length++;
        }
    }
}