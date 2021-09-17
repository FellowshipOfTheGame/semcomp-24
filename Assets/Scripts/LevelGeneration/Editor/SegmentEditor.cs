using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Segment))]
public class SegmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty width = serializedObject.FindProperty("width");
        SerializedProperty length = serializedObject.FindProperty("length");
        SerializedProperty segmentArray = serializedObject.FindProperty("segment");

        // Resizes array if size changes.
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(width);
        EditorGUILayout.PropertyField(length);
        if (EditorGUI.EndChangeCheck())
        {
            segmentArray.ClearArray();
            segmentArray.arraySize = width.intValue * length.intValue;
            serializedObject.ApplyModifiedProperties();
        }
        
        SerializedProperty[] segment = serializedObject.FindProperty("segment").ArrayValue();
        //Print array as matrix.
        for (int i = 0; i < length.intValue; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < width.intValue; j++)
            {
                EditorGUILayout.PropertyField(segment[i * width.intValue + j], GUIContent.none);
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}