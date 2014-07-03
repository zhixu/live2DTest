using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(Analysis))]
public class AnalysisDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		
		float height = 32+16+16+16;
		return height;
	}
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
				
		EditorGUI.BeginProperty (position, label, property);
		
		Rect nameRect = new Rect (position.x, position.y, 80, 16);
		EditorGUI.LabelField(nameRect,"Name");
		nameRect.x+=40;
		EditorGUI.PropertyField (nameRect, property.FindPropertyRelative ("name"),GUIContent.none);	
		
		position.y+=16;
		
		int e = property.FindPropertyRelative("end").intValue;
		int s = property.FindPropertyRelative("start").intValue;
		Rect curvePos = new Rect(position.x+45,position.y,160,32);
		Rect curveRect = new Rect(s,0,e-s,2);
		SerializedProperty curveProperty = property.FindPropertyRelative("weightCurve");
		EditorGUI.CurveField(curvePos,curveProperty,Color.yellow,curveRect);
		position.y+=32;
		
		Rect startRect = new Rect (position.x, position.y, 80, 16);
		Rect endRect = new Rect (position.x+140, position.y, 80, 16);
		EditorGUI.PropertyField (startRect, property.FindPropertyRelative ("start"),GUIContent.none);
		EditorGUI.PropertyField (endRect, property.FindPropertyRelative ("end"),GUIContent.none);
		
		EditorGUI.EndProperty ();
	}
	
}
