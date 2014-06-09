using UnityEngine;
using System.Collections;
using System.Collections.Generic;

interface ModelSetting 
{
	

	
	string getModelName()		 ;
	string getModelFile()		 ;

	
	int getTextureNum()			 ;
	string getTextureFile(int n) ;
	string[] getTextureFiles() ;

	
	int getHitAreasNum()		;
	string getHitAreaID(int n)	;
	string getHitAreaName(int n);

	
	string getPhysicsFile()	;
	string getPoseFile() ;
	int getExpressionNum() ;
	string getExpressionFile(int n) ;
	string[] getExpressionFiles() ;
	string getExpressionName(int n) ;
	string[] getExpressionNames() ;

	
	string[] getMotionGroupNames()	;
	int getMotionNum(string name)	;

	string getMotionFile(string name,int n) ;
	string getMotionSound(string name,int n) ;
	int getMotionFadeIn(string name,int n) ;
	int getMotionFadeOut(string name,int n) ;

	
	bool getLayout(Dictionary<string, float> layout) ;
	
	
	int getInitParamNum();
	float getInitParamValue(int n);
	string getInitParamID(int n);

	
	int getInitPartsVisibleNum();
	float getInitPartsVisibleValue(int n);
	string getInitPartsVisibleID(int n);
	
}