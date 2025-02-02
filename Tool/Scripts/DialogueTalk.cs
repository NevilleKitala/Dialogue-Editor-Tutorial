﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor.Dialogue.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class DialogueTalk : DialogueGetData
    {
        private AudioSource audioSource;

        private DialogueData currentDialogueNodeData;
        private BaseData lastDialogueNodeData;

        private DialogueMathCalculatorCondition DMCCondition = new DialogueMathCalculatorCondition();
        private DialogueMathCalculatorModifier DMCModifier = new DialogueMathCalculatorModifier();

        private List<DialogueData_Sentence> paragraph;

        private Action nextNodeCheck;
        
        private bool runCheck;
        public bool teletypeCheck = false;

        private IEnumerator teletype;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (runCheck == true)
            {
                runCheck = false;
                nextNodeCheck.Invoke();
            }
        }

        public void StartDialogue()
        {
            if (dialogueContainerSO.StartData != null && lastDialogueNodeData == null)
                CheckNodeType(GetNextNode(dialogueContainerSO.StartData));
            else if (lastDialogueNodeData != null)
            {
                CheckNodeType(GetNextNode(dialogueContainerSO.StartData));
            }
            else
                Debug.Log($"<color=red>Error: </color>Your Dialogue Object Must have a start Node.");
        }

        public void ContinueDialogue()
        {

        }

        public void StopDialogue()
        {
            DialogueController.Instance.ShowDialogueUI(false);
            DialogueController.Instance.text.text = string.Empty;
            if(teletype != null)
                StopCoroutine(teletype);
        }

        private void CheckNodeType(BaseData _baseNodeData)
        {
            switch (_baseNodeData)
            {
                case StartData nodeData:
                    RunNode(nodeData);
                    lastDialogueNodeData = nodeData;
                    break;
                case DialogueData nodeData:
                    RunNode(nodeData);
                    break;
                case EventData nodeData:
                    RunNode(nodeData);
                    break;
                case EndData nodeData:
                    RunNode(nodeData);
                    break;
                case BranchData nodeData:
                    RunNode(nodeData);
                    break;
                case ModifierData nodeData:
                    RunNode(nodeData);
                    break;
                default:
                    break;
            }
        }

        private void RunNode(StartData nodeData)
        {
            CheckNodeType(GetNextNode(dialogueContainerSO.StartData));
        }

        private void RunNode(BranchData nodeData)
        {
            bool checkBranch = false;
            foreach (EventData_StringCondition item in nodeData.EventData_StringConditions)
            {
                if (DMCCondition.StringCondition(item))
                {
                    checkBranch = true;
                    break;
                }
            }
            foreach (EventData_FloatCondition item in nodeData.EventData_FloatConditions)
            {
                if (DMCCondition.FloatCondition(item))
                {
                    checkBranch = true;
                    break;
                }
            }
            foreach (EventData_IntCondition item in nodeData.EventData_IntConditions)
            {
                if (DMCCondition.IntCondition(item))
                {
                    checkBranch = true;
                    break;
                }
            }
            foreach (EventData_BoolCondition item in nodeData.EventData_BoolConditions)
            {
                if (DMCCondition.BoolCondition(item))
                {
                    checkBranch = true;
                    break;
                }
            }

            string nextNoce = (checkBranch ? nodeData.trueGuidNode : nodeData.falseGuidNode);
            nextNodeCheck = () => { CheckNodeType(GetNodeByGuid(nextNoce)); };
            runCheck = true;
        }

        private void RunNode(EventData nodeData)
        {
            foreach (Container_DialogueEventSO item in nodeData.Container_DialogueEventSOs)
            {
                if (item.GameEvent != null)
                {
                    item.GameEvent.Raise();
                }
            }
            nextNodeCheck = () =>
            {
                CheckNodeType(GetNextNode(nodeData));
            };
            runCheck = true;
        }

        private void RunNode(ModifierData nodeData)
        {
            foreach (ModifierData_String item in nodeData.ModifierData_Strings)
            {
                DMCModifier.StringModifier(item);
            }
            foreach (ModifierData_Float item in nodeData.ModifierData_Floats)
            {
                DMCModifier.FloatModifier(item);
            }
            foreach (ModifierData_Int item in nodeData.ModifierData_Ints)
            {
                DMCModifier.IntModifier(item);
            }
            foreach (ModifierData_Bool item in nodeData.ModifierData_Bools)
            {
                DMCModifier.BoolModifier(item);
            }
            nextNodeCheck = () =>
            {
                CheckNodeType(GetNextNode(nodeData));
            };
            runCheck = true;
        }

        private void RunNode(EndData nodeData)
        {
            switch (nodeData.EndNodeType.Value)
            {
                case EndNodeType.End:
                    DialogueController.Instance.ShowDialogueUI(false);
                    break;
                case EndNodeType.Repeat:
                    nextNodeCheck = () =>
                    {
                        CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
                    }; runCheck = true;
                    break;
                case EndNodeType.ReturnToStart:
                    nextNodeCheck = () =>
                    {
                        CheckNodeType(GetNextNode(dialogueContainerSO.StartData));
                    }; runCheck = true;
                    break;
                default:
                    break;
            }
        }

        private void RunNode(DialogueData nodeData)
        {
            currentDialogueNodeData = nodeData;

            DialogueController.Instance.ShowDialogueUI(false);
            if(paragraph != null)
                paragraph.Clear();
            else
                paragraph = new List<DialogueData_Sentence>();
            paragraph.AddRange(nodeData.DialogueData_Text.sentence);
            
            DialogueController.Instance.SetName(nodeData.DialogueData_DialogueAssets.actor.dialogueAssetsName);
            DialogueToDo();
        }

        private void DialogueToDo()
        {
            List<Sentence> parsedParagraph = new List<Sentence>();
            int sentenceCounter = 0;

            DialogueController.Instance.counter = 0;
            DialogueController.Instance.totalVisibleCharacters = 0;
            DialogueController.Instance.text.text = "";

            foreach (DialogueData_Sentence sentence in paragraph)
            {
                Sentence currentSentence = new Sentence();
                currentSentence.sentence = " " + sentence.Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
                currentSentence.volume = sentence.volumeType.Value;
                currentSentence.pauseAtPunctuation = sentence.pauseAtPunctuation.Value;
                parsedParagraph.Add(currentSentence);
            }

            if (currentDialogueNodeData.DialogueData_Text.Sprite_Left.Value)
                DialogueController.Instance.SetLeftImage(currentDialogueNodeData.DialogueData_Text.Sprite_Left.Value);
            if (currentDialogueNodeData.DialogueData_Text.Sprite_Right.Value)
                DialogueController.Instance.SetRightImage(currentDialogueNodeData.DialogueData_Text.Sprite_Right.Value);

            DialogueController.Instance.ShowDialogueUI(true);
            DialogueController.Instance.SetDynamicSentence(parsedParagraph[0]);
            DialogueController.Instance.text.maxVisibleCharacters = 0;

            teletype = TeletypeRework(sentenceCounter, parsedParagraph);
            StartCoroutine(teletype);
        }

        private IEnumerator TeletypeRework(int sentenceCounter, List<Sentence> parsedParagraph)
        {
            teletypeCheck = true;
            int lastTotal = DialogueController.Instance.totalVisibleCharacters - (parsedParagraph[sentenceCounter].sentence.Length - 1);
            while (DialogueController.Instance.counter <= DialogueController.Instance.totalVisibleCharacters)
            {
                if (!teletypeCheck)
                {
                    DialogueController.Instance.text.maxVisibleCharacters = DialogueController.Instance.text.textInfo.characterCount;
                    DialogueController.Instance.SetFullText(parsedParagraph);
                    yield break;
                }

                int index = DialogueController.Instance.counter - lastTotal;
                index = index < 0 ? 0 : index;

                Debug.Log($"position: {index} is {parsedParagraph[sentenceCounter].sentence[index]}");

                DialogueController.Instance.counter++;

                DialogueController.Instance.text.maxVisibleCharacters = DialogueController.Instance.counter;

                int lastLine = DialogueController.Instance.text.textInfo.lineCount;


                if (parsedParagraph[sentenceCounter].sentence[index] == '.' ||
                    parsedParagraph[sentenceCounter].sentence[index] == ',' ||
                    parsedParagraph[sentenceCounter].sentence[index] == '?' ||
                    parsedParagraph[sentenceCounter].sentence[index] == '!')
                {
                    yield return new WaitForSeconds(parsedParagraph[sentenceCounter].pauseAtPunctuation);
                }
                else
                {
                    yield return new WaitForSeconds(DialogueController.Instance.teletypeInterval);
                }
            }

            sentenceCounter++;
            if(sentenceCounter >= parsedParagraph.Count)
                teletypeCheck = false;
            else
            {
                IEnumerator temp = teletype;
                DialogueController.Instance.SetDynamicSentence(parsedParagraph[sentenceCounter]);
                teletype = TeletypeRework(sentenceCounter, parsedParagraph);
                StartCoroutine(teletype);
                StopCoroutine(temp);
            }
        }

        public void GetNext()
        {
            DialogueController.Instance.counter = 0;
            DialogueController.Instance.totalVisibleCharacters = 0;
            CheckNodeType(GetNextNode(currentDialogueNodeData));
        }

        public void GetFinish()
        {
            teletypeCheck = false;
        }
    }
}