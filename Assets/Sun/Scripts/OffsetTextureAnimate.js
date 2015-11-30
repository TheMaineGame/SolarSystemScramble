// Scroll main texture based on time
//v1.01
//-IOS compatible
var scrollSpeedX = 0.015;
var scrollSpeedY = 0.015;
var scrollSpeedXMaterial2 = 0.015;
var scrollSpeedYMaterial2 = 0.015;
function Update () {
    var offsetX = Time.time * scrollSpeedX%1;
    var offsetY = Time.time * scrollSpeedY%1;
    var offset2X = Time.time * scrollSpeedXMaterial2%1;
    var offset2Y = Time.time * scrollSpeedYMaterial2%1;
    GetComponent.<Renderer>().material.SetTextureOffset ("_BumpMap", Vector2(offsetX,offsetY));
    GetComponent.<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(offsetX,offsetY));
    if(GetComponent.<Renderer>().materials.Length>1){
   		 GetComponent.<Renderer>().materials[1].SetTextureOffset ("_MainTex", Vector2(offset2X,offset2Y));
  		 GetComponent.<Renderer>().materials[1].SetTextureOffset ("_BumpMap", Vector2(offset2X,offset2Y));
    }
}