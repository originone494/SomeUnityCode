using ARPG.UI;
using Assets.Scripts;
using UnityEngine;

namespace ARPG.Dialogue
{
    public class NPC : InteractableBase
    {
        [SerializeField] private Sprite LeftPic;
        [SerializeField] private Sprite RightPic;
        [SerializeField] private DialogueSetting[] content;

        protected override void Interact()
        {
            Debug.Log("NPC interact");
            DialogueSettings dialogue = new DialogueSettings(LeftPic, RightPic, content);
            BasePanel basePanel = UIManager.Instance.OpenPanel(UIConst.DialoguePanel);
            DialogSystem.Instance.interact(dialogue, basePanel);
        }
    }
}