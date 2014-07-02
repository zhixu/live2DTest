using UnityEngine;
using System.Collections;
using System.IO;
using live2d;

public class DisplayAvatar : MonoBehaviour {
	public TextAsset mocFile;
	public Texture2D[] textures;

	private Live2DModelUnity live2DModel;

	// Use this for initialization
	void Start () {
		Live2D.init();

		live2DModel = Live2DModelUnity.loadModel(mocFile.bytes); //loads the Live2D model

		for (int i = 0; i<textures.Length; i++) {
			live2DModel.setTexture(i, textures[i]);
		}


		


		/*
		modelMatrix = new L2DModelMatrix(live2DModel.getCanvasWidth(), live2DModel.getCanvasHeight());
        modelMatrix.setWidth(2);
		modelMatrix.multScale(1,1,-1);
        modelMatrix.setCenterPosition(0, 0);*/
	}

	void OnRenderObject() {
		Matrix4x4 m1 = Matrix4x4.Ortho(
                        -200.0f, 200.0f,
                         200.0f,-200.0f,
                        -0.5f,0.5f);
        Matrix4x4 m2 = transform.localToWorldMatrix;
        Matrix4x4 m3 = m2*m1;
       
        live2DModel.setMatrix(m3);
        live2DModel.update();
        live2DModel.draw();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
