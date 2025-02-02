﻿using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DialogueEditor.Dialogue
{
    [System.Serializable]
    public class DialogueData : BaseData
    {
        public DialogueData_Text DialogueData_Text = new DialogueData_Text();
        public Container_Actor DialogueData_DialogueAssets = new Container_Actor();
    }

    [System.Serializable]
    public class DialogueData_BaseContainer
    {
        public Container_Int ID = new Container_Int();
    }

// Sentences are displayed at once per paragraph
    [System.Serializable]
    public class DialogueData_Sentence
    {
#if UNITY_EDITOR
        public TextField TextField { get; set; }
        public Container_VolumeType volumeType = new Container_VolumeType();
#endif
        public Container_String GuidID = new Container_String();
        public List<LanguageGeneric<string>> Text = new List<LanguageGeneric<string>>();
        public Container_Float pauseAtPunctuation = new Container_Float();
    }

    [System.Serializable]
    public class SentencePause
    {
#if UNITY_EDITOR
#endif

    }

    [System.Serializable]
    public class DialogueData_Text : DialogueData_BaseContainer
    {
#if UNITY_EDITOR
        public List<DialogueData_Sentence> sentence = new List<DialogueData_Sentence>();
        //public ObjectField ObjectField { get; set; }
#endif
        public Container_String GuidID = new Container_String();
        //public List<LanguageGeneric<AudioClip>> AudioClips = new List<LanguageGeneric<AudioClip>>();

        public Container_Sprite Sprite_Left = new Container_Sprite();
        public Container_Sprite Sprite_Right = new Container_Sprite();
    }

    [System.Serializable]
    public class DialogueData_Port
    {
        public string PortGuid;
        public string InputGuid;
        public string OutputGuid;
    }
}
