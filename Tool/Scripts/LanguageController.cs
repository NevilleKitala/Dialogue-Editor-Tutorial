using UnityEngine;

namespace DialogueEditor.Dialogue.Scripts
{
    public class LanguageController : MonoBehaviour
    {
        [SerializeField] private LanguageType language;

        public static LanguageController Instance { get; private set; }
        public LanguageType Language { get => language; set => language = value; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void GetLocale(string locale)
        {
            switch (locale)
            {
                case "English":
                    Language = LanguageType.English;
                    break;
                case "Swahili":
                    Language = LanguageType.English;
                    break;
                case "French":
                    Language = LanguageType.French;
                    break;
                case "German":
                    Language = LanguageType.German;
                    break;
                case "Italian":
                    Language = LanguageType.Italian;
                    break;
            }
        }
    }


}