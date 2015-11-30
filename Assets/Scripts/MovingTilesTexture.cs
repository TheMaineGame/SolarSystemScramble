//http://answers.unity3d.com/questions/19848/making-textures-scroll-animate-textures.html

using UnityEngine;
using System.Collections;

public class MovingTilesTexture : MonoBehaviour 
{
	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 0.05f, 0.0f );
	public string textureName = "_MainTex";
	public string textureName2 = "_SpecMap";
	Vector2 uvOffset = Vector2.zero;
	
	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
			GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName2, uvOffset );
		}
	}
}