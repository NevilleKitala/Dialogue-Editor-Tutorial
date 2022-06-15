using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogueEditor.Dialogue.Scripts
{
    public class DialogueController: MonoBehaviour
    {
        public static DialogueController _instance;
        public float teletypeInterval = 0.05f;
        public TextMeshProUGUI text;
        public int totalVisibleCharacters;
        public int counter = 0;

        public static DialogueController Instance{
            get {
                if(_instance is null)
                    Debug.LogError("DialogueController is not in the Scene: Add The dialogue Assets Prefab to your Scene");
                return _instance;
            }
        }

        private void Awake() {
            _instance = this;
        }
        
        public void ShowDialogueUI(bool show)
        {
            DialogueAssets.Instance.dialogueUI.SetActive(show);
        }

        public void SetText(string text)
        {
            DialogueAssets.Instance.textBox.GetComponent<TextMeshProUGUI>().text += text;
        }

        public void SetFullText(List <Sentence> paragraph)
        {
            text = DialogueAssets.Instance.textBox.GetComponent<TextMeshProUGUI>();
            text.textInfo.Clear();
            text.text = "";
            for (int i = 0; i < paragraph.Count; i++)
            {
                totalVisibleCharacters += paragraph[i].sentence.Length;
                switch(paragraph[i].volume){
                    case VolumeType.Neutral:
                        text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.neutral)}>{paragraph[i].sentence}</color>";
                        break;
                    case VolumeType.Shout:
                        text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.shout)}>{paragraph[i].sentence}</color>";
                        break;
                    case VolumeType.Drunk:
                        text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.drunk)}>{paragraph[i].sentence}</color>";
                        break;
                    case VolumeType.Whisper:
                        text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.whisper)}>{paragraph[i].sentence}</color>";
                        break;
                    case VolumeType.Tired:
                        text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.tired)}>{paragraph[i].sentence}</color>";
                        break;
                    case VolumeType.Special:
                        text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.special)}>{paragraph[i].sentence}</color>";
                        break;
                }
            }
        }

        public float SetDynamicSentence(Sentence sentence)
        {
            totalVisibleCharacters += sentence.sentence.Length;
            switch (sentence.volume)
            {
                case VolumeType.Neutral:
                        text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.neutral)}>{sentence.sentence}</color>";
                    break;
                case VolumeType.Shout:
                    text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.shout)}>{sentence.sentence}</color>";
                    break;
                case VolumeType.Drunk:
                    text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.drunk)}>{sentence.sentence}</color>";
                    break;
                case VolumeType.Whisper:
                    text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.whisper)}>{sentence.sentence}</color>";
                    break;
                case VolumeType.Tired:
                    text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.tired)}>{sentence.sentence}</color>";
                    break;
                case VolumeType.Special:
                    text.text += $"<color={ColorUtility.ToHtmlStringRGB(DialogueAssets.Instance.special)}>{sentence.sentence}</color>";
                    break;
            }

            return sentence.pauseAtPunctuation;
        }


        public void SetName(string text)
        {
            DialogueAssets.Instance.textName.GetComponent<TextMeshProUGUI>().text = text;
        }

        public void SetLeftImage(Sprite leftImage)
        {
            if (leftImage != null)
                DialogueAssets.Instance.leftImage.sprite = leftImage;
        }

        public void SetRightImage(Sprite rightImage)
        {

            if (rightImage != null)
                DialogueAssets.Instance.rightImage.sprite = rightImage;
        }
    }

    public class Sentence
    {
        public string sentence { get; set; }
        public float pauseAtPunctuation { get; set; }
        public VolumeType volume { get; set; }
        public Sentence()
        {

        }

    }

}