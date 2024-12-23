using ARPG.Dialogue;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ARPG.UI
{
    public class DialoguePanel : BasePanel
    {
        public GameObject PicLeft;
        public GameObject PicLeftGray;
        public GameObject PicRight;
        public GameObject PicRightGray;

        public GameObject LeftName;
        public GameObject LeftNameBG;
        public GameObject RightName;
        public GameObject RightNameBG;

        public GameObject Text;

        private TextMeshProUGUI TextMeshProUGUI;
        private TextMeshProUGUI Left_TextMeshProUGUI;
        private TextMeshProUGUI Right_TextMeshProUGUI;

        private void Awake()
        {
            TextMeshProUGUI = Text.GetComponent<TextMeshProUGUI>();
            Left_TextMeshProUGUI = LeftName.GetComponent<TextMeshProUGUI>();
            Right_TextMeshProUGUI = RightName.GetComponent<TextMeshProUGUI>();
        }

        public void Init(Sprite left, Sprite right)
        {
            PicLeft.GetComponent<Image>().sprite = left;
            PicLeftGray.GetComponent<Image>().sprite = left;

            PicRight.GetComponent<Image>().sprite = right;
            PicRightGray.GetComponent<Image>().sprite = right;
        }

        public void display(string name, string text, DialogueHuman human)
        {
            if (human == DialogueHuman.Left)
            {
                PicLeft.SetActive(true);
                PicLeftGray.SetActive(false);

                PicRight.SetActive(false);
                PicRightGray.SetActive(true);

                LeftName.SetActive(true);
                LeftNameBG.SetActive(true);
                RightName.SetActive(false);
                RightNameBG.SetActive(false);
                TextMeshProUGUI.text = text;
                Left_TextMeshProUGUI.text = name;
            }
            else if (human == DialogueHuman.Right)
            {
                PicLeft.SetActive(false);
                PicLeftGray.SetActive(true);

                PicRight.SetActive(true);
                PicRightGray.SetActive(false);

                LeftName.SetActive(false);
                LeftNameBG.SetActive(false);
                RightName.SetActive(true);
                RightNameBG.SetActive(true);
                TextMeshProUGUI.text = text;
                Right_TextMeshProUGUI.text = name;
            }
        }
    }
}