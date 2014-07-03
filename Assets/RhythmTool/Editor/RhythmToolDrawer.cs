using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(RhythmTool))]
public class RhythmToolDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		
		float height = 16;
		
		if(property.isExpanded)
		{
			height=16+16+16+16+16+16+16;
			
			
			
			SerializedProperty advancedAnalyses = property.FindPropertyRelative("advancedAnalyses");
			SerializedProperty analyses = property.FindPropertyRelative("analyses");
			if(analyses.isExpanded&&advancedAnalyses.boolValue)
			{
				height+=EditorGUI.GetPropertyHeight(analyses);
			}
			else if(advancedAnalyses.boolValue) 
				height+=0;//16;
			else
				height+=16+16+16;
			
		}		
		return height;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		
		GUI.skin = EditorGUIUtility.Load("LevelEditor/LevelEditorSkin.guiskin") as GUISkin;
			
		position.height = 16f;
		RhythmToolFoldout(position, property, label);
		position.y += 16f;
		
		if (!property.isExpanded) {
			return;
		}
		
		EditorGUI.indentLevel += 1;
		
		SerializedProperty totalFrames = property.FindPropertyRelative("totalFrames");
		EditorGUI.LabelField(position,"Total Frames: ");
		position.x+=100;
		EditorGUI.LabelField(position,totalFrames.intValue.ToString());
		position.x-=100;
		position.y += 16f;
		
		SerializedProperty currentFrame = property.FindPropertyRelative("currentFrame");
		EditorGUI.LabelField(position,"Current Frame: ");
		position.x+=100;
		EditorGUI.LabelField(position,currentFrame.intValue.ToString());
		position.x-=100;
		position.y += 16f;		
		
		SerializedProperty lastFrame = property.FindPropertyRelative("lastFrame");
		EditorGUI.LabelField(position,"Last Frame: ");	
		position.x+=100;
		EditorGUI.LabelField(position,lastFrame.intValue.ToString());
		position.x-=100;
		position.y += 16f;
		
		SerializedProperty lead = property.FindPropertyRelative("lead");
		EditorGUI.IntSlider(position,lead,40,1000);
		position.y += 16f;
		
		
		SerializedProperty advancedAnalyses = property.FindPropertyRelative("advancedAnalyses");
		EditorGUI.BeginDisabledGroup(Application.isPlaying);
		EditorGUI.LabelField(position,"Advanced");
		position.x+=100;
		EditorGUI.PropertyField(position,advancedAnalyses,GUIContent.none);	
		EditorGUI.EndDisabledGroup();
		position.x-=100;
		
			
		if(!advancedAnalyses.boolValue){
			
			position.y += 16f;
			EditorGUI.LabelField(position,"Analyses");
			position.y += 16f;
			position.x += 16f;
			EditorGUI.LabelField(position,"Low");
			position.y += 16f;
			EditorGUI.LabelField(position,"Mid");
			position.y += 16f;
			EditorGUI.LabelField(position,"High");
			return;
		}
		position.y += 16f;
		SerializedProperty analyses = property.FindPropertyRelative("analyses");
		EditorGUI.PropertyField(position, analyses);		
		position.y += 16f;
				
		EditorGUI.indentLevel -= 1;
	}
	
	private void RhythmToolFoldout (Rect position, SerializedProperty property, GUIContent label) {
		//position.x -= 14f;
		position.width += 14f;
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
		EditorGUI.EndProperty();
	}
}
