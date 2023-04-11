using UnityEditor;
using UnityEngine;

namespace Code.Utils.Hide.Editor
{
    [CustomPropertyDrawer(typeof(HideAttribute))]
    public class HideDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
                EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
                return EditorGUI.GetPropertyHeight(property, label, true);

            return -EditorGUIUtility.standardVerticalSpacing;
        }
        
        bool ShouldShow(SerializedProperty property)
        {
            HideAttribute hideAttribute = (HideAttribute)attribute;

            SerializedProperty conditionProperty = property.serializedObject.FindProperty(hideAttribute.Flag);

            if (conditionProperty == null || conditionProperty.type != "bool")
                return true;

            return !conditionProperty.boolValue;
        }
    }
}