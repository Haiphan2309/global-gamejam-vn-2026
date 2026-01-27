using UnityEditor;
using UnityEditor.UI;

namespace UI
{
    [CustomEditor(typeof(UICustomButton))]
    public class UICustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom States", EditorStyles.boldLabel);

            serializedObject.Update();

            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("m_normalTemplate"));
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("m_hoverTemplate"));
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("m_pressTemplate"));
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("m_disabledTemplate"));
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("m_selectedTemplate"));
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("m_isSetStateManual"));
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("m_backgroundPatternSprite"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
