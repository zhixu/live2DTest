using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(AnalysisListAttribute))]
public class AnalysisListDrawer : PropertyDrawer
{

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		if (!property.isExpanded) {
			return 16f;
		}
		float height = 48f;
		for (int i = 0; i < property.arraySize; i++) {
			height += EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i));
		}
		return height;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		position.height = 16f;
		ShowFoldout(position, property, label);
		position.y += 16f;
		
		if (!property.isExpanded) {
			return;
		}
		
		EditorGUI.indentLevel += 1;
		
		Rect rect = new Rect (position.x, position.y, 80, position.height);		
		EditorGUI.LabelField(rect,"Size");
		
		rect.x+=40;
		SerializedProperty arraySize = property.FindPropertyRelative("Array.size");
		EditorGUI.PropertyField(rect, arraySize,GUIContent.none);
		arraySize.intValue=Mathf.Clamp(arraySize.intValue,0,10);
		position.y += 32;
		
		ShowElements(position, property);
		
		EditorGUI.indentLevel -= 1;
	}
	
	private void ShowFoldout (Rect position, SerializedProperty property, GUIContent label) {
		//position.x -= 14f;
		position.width += 14f;
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
		EditorGUI.EndProperty();
	}
	
	private void ShowElements (Rect position, SerializedProperty property) {
		for (int i = 0; i < property.arraySize; i++) {
			SerializedProperty element = property.GetArrayElementAtIndex(i);
			position.height = EditorGUI.GetPropertyHeight(element);
			EditorGUI.PropertyField(position, element, true);
			position.y += position.height;
		}
	}
	
	
}
