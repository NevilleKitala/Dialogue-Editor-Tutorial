﻿using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace DialogueEditor.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue Object")]
    [System.Serializable]
    public class DialogueContainerSO : ScriptableObject
    {

        public int participatingDialogueAssetss;
        public List<NodeLinkData> NodeLinkData = new List<NodeLinkData>();

        public EndData EndData = new EndData();
        public StartData StartData = new StartData();
        public List<EventData> EventData = new List<EventData>();
        public List<ModifierData> ModifierData = new List<ModifierData>();
        public List<BranchData> BranchData = new List<BranchData>();
        public List<DialogueData> DialogueData = new List<DialogueData>();

        public List<ScriptableObject> variables = new List<ScriptableObject>();

        public List<BaseData> AllData
        {
            get
            {
                List<BaseData> tmp = new List<BaseData>();
                tmp.Add(EndData);
                tmp.Add(StartData);
                tmp.AddRange(EventData);
                tmp.AddRange(ModifierData);
                tmp.AddRange(BranchData);
                tmp.AddRange(DialogueData);

                return tmp;
            }
        }

        [SerializeField, HideInInspector] private bool _hasBeenInitialised = false;

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            if (!_hasBeenInitialised)
            {
                // do your initialisation :)
                _hasBeenInitialised = true;
                if(StartData.ParticipatingActors.Count == 0)
                {

                }
            }
        }

        private void OnDestroy()
        {
            foreach(ScriptableObject so in variables)
            {
                if(so != null)
                    Destroy(so);
            }
        }
    }


    [System.Serializable]
    public class NodeLinkData
    {
        public string BaseNodeGuid;
        public string BasePortName;
        public string TargetNodeGuid;
        public string TargetPortName;
    }
}