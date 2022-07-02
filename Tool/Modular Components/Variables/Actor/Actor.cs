using UnityEngine;

namespace DialogueEditor.ModularComponents
{
    public class Actor : ScriptableObject
    {
        [Header("DialogueAssets Details")]
        public string dialogueAssetsName;
        public ActorType actorType;

        public bool actorSpeaking = false;

        public static Actor NewActor(ScriptableObject so, string name)
        {
            Actor newActor = ScriptableObject.CreateInstance<Actor>();

            newActor.name = name;
            newActor.dialogueAssetsName = newActor.name;
            newActor.actorType = ActorType.NPC;
            return newActor;
        }
    }


    public enum ActorType
    {
        Player,
        NPC
    }
}
