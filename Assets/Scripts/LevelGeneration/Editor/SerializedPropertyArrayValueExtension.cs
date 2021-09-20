using System;
using UnityEditor;
using UnityEngine;

public static class SerializedPropertyArrayValueExtension
{
    public static SerializedProperty[] ArrayValue(this SerializedProperty serializedProperty)
    {
        if (!serializedProperty.isArray)
        {
            Debug.LogError("Property isn't array type.");
            return null;
        }

        int length = serializedProperty.arraySize;
        SerializedProperty[] objectArray = new SerializedProperty[length];
        for (int i = 0; i < length; i++)
        {
            objectArray[i] = serializedProperty.GetArrayElementAtIndex(i);
        }
        
        return objectArray;
    }
}
