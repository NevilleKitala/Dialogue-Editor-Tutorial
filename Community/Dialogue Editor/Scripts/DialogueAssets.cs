using DialogueEditor.ModularComponents;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueAssets : MonoBehaviour
{
    public static DialogueAssets _instance;

    public Color neutral = Color.white;
    public Color whisper = Color.grey;
    public Color shout = Color.red;
    public Color drunk = Color.blue;
    public Color tired = Color.magenta;
    public Color special = Color.cyan;

    public static DialogueAssets Instance{
        get {
            if(_instance is null)
                Debug.LogError("DialogueAssets are not in the Scene: Add The dialogue Assets Prefab to your Scene");
            return _instance;
        }
    }

    private void Awake() {
        if (_instance is null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            _instance.dialogueUI = this.dialogueUI;
            _instance.textName = this.textName;
            _instance.textBox = this.textBox;
            _instance.leftImage = this.leftImage;
            _instance.rightImage = this.rightImage;
            _instance.activeChoice = this.activeChoice;

            Destroy(gameObject)
        }


    }

    [Header("DialogueAssets Details")]
    [SerializeField] public GameObject dialogueUI;

    [Header("Text")]
    [SerializeField] public TextMeshProUGUI textName;
    [SerializeField] public TextMeshProUGUI textBox;

    [Header("Image")]
    [SerializeField] public Image leftImage;
    [SerializeField] public Image rightImage;

    public Button activeChoice;
    public UnityEvent continueEvent;

    public void choiceSelect(){
        activeChoice.onClick.Invoke();
    }
}
