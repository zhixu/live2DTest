/**
 *
 *  このソースはLive2D関連アプリの開発用途に限り
 *  自由に改変してご利用頂けます。
 *
 *  (c) CYBERNOIDS Co.,Ltd. All rights reserved.
 */
using System.Collections;

/**
 * モデルの位置指定に使うと便利な行列
 */
public class L2DModelMatrix : L2DMatrix44
{
	private float width;//モデルのサイズ
	private float height;

	public L2DModelMatrix(float w,float h)
	{
		width=w;
		height=h;
	}

	public void setPosition(float x,float y)
	{
		translate(x, y);
	}


	public void setCenterPosition(float x,float y)
	{
		float w=width * getScaleX();
		float h=height* getScaleY();
		translate(x-w/2, y-h/2);
	}


	public void top(float y)
	{
		setY(y);
	}


	public void bottom(float y)
	{
		float h=height* getScaleY();
		translateY( y-h);
	}


	public void left(float x)
	{
		setX(x);
	}


	public void right(float x)
	{
		float w=width * getScaleX();
		translateX(x-w);
	}


	public void centerX(float x)
	{
		float w=width * getScaleX();
		translateX(x-w/2);
	}


	public void centerY(float y)
	{
		float h=height* getScaleY();
		translateY( y-h/2);
	}


	public void setX(float x)
	{
		translateX(x);
	}


	public void setY(float y)
	{
		translateY(y);
	}


	/**
	 * 縦幅をもとにしたサイズ変更
	 * 縦横比はもとのまま
	 * @param h
	 */
	public void setHeight(float h)
	{
		float scaleX = h/height;
		float scaleY = - scaleX ;
		scale(scaleX, scaleY);
	}


	/**
	 * 横幅をもとにしたサイズ変更
	 * 縦横比はもとのまま
	 * @param w
	 */
	public void setWidth(float w)
	{
		float scaleX = w/width;
		float scaleY = - scaleX ;
		scale(scaleX, scaleY);
	}
}