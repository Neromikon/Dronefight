using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

[CustomEditor(typeof(LocalizedText))]
public class LocalizedTextEditorEditor : Editor
{
	SerializedProperty chineseProperty;
	SerializedProperty englishProperty;
	SerializedProperty greekProperty;
	SerializedProperty russianProperty;
	SerializedProperty ukrainianProperty;

	public void OnEnable()
	{
		chineseProperty = serializedObject.FindProperty("chinese");
		englishProperty = serializedObject.FindProperty("english");
		greekProperty = serializedObject.FindProperty("greek");
		russianProperty = serializedObject.FindProperty("russian");
		ukrainianProperty = serializedObject.FindProperty("ukrainian");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		LocalizedText localizedText = (LocalizedText)target;

		EditorGUILayout.PrefixLabel("chinese:");
		chineseProperty.stringValue = EditorGUILayout.TextField(localizedText.chinese, GUILayout.Height(80.0f));

		EditorGUILayout.PrefixLabel("english:");
		englishProperty.stringValue = EditorGUILayout.TextArea(localizedText.english, GUILayout.Height(80.0f));

		EditorGUILayout.PrefixLabel("greek:");
		greekProperty.stringValue = EditorGUILayout.TextArea(localizedText.greek, GUILayout.Height(80.0f));

		EditorGUILayout.PrefixLabel("russian:");
		russianProperty.stringValue = EditorGUILayout.TextArea(localizedText.russian, GUILayout.Height(80.0f));

		EditorGUILayout.PrefixLabel("ukrainian:");
		ukrainianProperty.stringValue = EditorGUILayout.TextArea(localizedText.ukrainian, GUILayout.Height(80.0f));

		serializedObject.ApplyModifiedProperties();
	}
}

#endif