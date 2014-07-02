//
// Selected U3D Japanese fonts  sample scene's script
//
// the script encoding is UTF-8 with BOM
//

var customSkin:GUISkin;

private var buttonW:int = 200;
private var buttonH:int = 48;
private var buttonT:int = 60;

// draw GUI text
function OnGUI()
{
	var x:int = ( Screen.width / 2 ) - buttonW / 2;
	var y:int = ( Screen.height - buttonT * 3 ) /2;

	GUI.skin = customSkin;
	
	// draw words "start at first"
	if(GUI.Button(Rect(x, y + buttonT  * 0 ,buttonW,buttonH),"最初から始める"))
	{
	}
	
	// draw word "continue"
	if(GUI.Button(Rect(x, y + buttonT  * 1 ,buttonW,buttonH),"続きから"))
	{
	}

	// draw word "setting"
	if(GUI.Button(Rect(x, y + buttonT  * 2 ,buttonW,buttonH),"環境設定"))
	{
	}

}

// camera rotation
function Update () {

	transform.Rotate( 0,10.0 * Time.deltaTime,0 );
}
