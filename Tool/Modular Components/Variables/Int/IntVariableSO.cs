using UnityEngine;

namespace DialogueEditor.ModularComponents
{
    public class IntVariableSO : ScriptableObject
    {
#if UNITY_EDITOR
#pragma warning disable CS0414
        [Multiline]
        [SerializeField] private string _developerDescription = "";
#pragma warning restore CS0414
#endif
        [SerializeField] private int _value;

        public int Value { get => _value; set => _value = value; }

        public void SetValue(int value)
        {
            _value = value;
        }

        public void SetValue(IntVariableSO value)
        {
            _value = value._value;
        }

        public void ApplyChange(int amount)
        {
            _value += amount;
        }

        public void ApplyChange(IntVariableSO amount)
        {
            _value += amount._value;
        }

        public static IntVariableSO NewInt(ScriptableObject so, string name)
        {
            IntVariableSO newInt = ScriptableObject.CreateInstance<IntVariableSO>();
            newInt.name = name;
            return newInt;
        }
    }
}