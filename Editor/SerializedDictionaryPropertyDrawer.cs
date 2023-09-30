using UnityEditor;
using UnityEngine;

namespace ActionCode.SerializedDictionaries.Editor
{
    /// <summary>
    /// Draws the dictionary and a warning-box if there are duplicate keys.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>))]
    public class SerializedDictionaryPropertyDrawer : PropertyDrawer
    {
        private const float warningBoxHeight = 1.5f;
        private static readonly float lineHeight = EditorGUIUtility.singleLineHeight;
        private static readonly float vertSpace = EditorGUIUtility.standardVerticalSpacing;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var list = GetListProperty(property);
            var height = EditorGUI.GetPropertyHeight(list, includeChildren: true);

            // Height of key collision warning.
            var keyCollision = GetHasDuplicateKeys(property);
            if (keyCollision)
            {
                height += warningBoxHeight * lineHeight;
                if (!list.isExpanded) height += vertSpace;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw list of key/value pairs.
            var list = GetListProperty(property);
            EditorGUI.PropertyField(position, list, label, includeChildren: true);

            // Draw key collision warning.
            var keyCollision = GetHasDuplicateKeys(property);
            if (!keyCollision) return;

            position.y += EditorGUI.GetPropertyHeight(list, true);

            if (!list.isExpanded) position.y += vertSpace;

            position.height = lineHeight * warningBoxHeight;
            position = EditorGUI.IndentedRect(position);

            EditorGUI.HelpBox(position, "Duplicate keys will not be serialized.", MessageType.Error);
        }

        private static SerializedProperty GetListProperty(SerializedProperty property) =>
            property.FindPropertyRelative("list");

        private static bool GetHasDuplicateKeys(SerializedProperty property)
        {
            var fieldName = nameof(SerializedDictionary<object, object>.HasDuplicateKeys);
            var serializedFieldName = $"<{fieldName}>k__BackingField";
            var field = property.FindPropertyRelative(serializedFieldName);
            return field.boolValue;
        }
    }
}