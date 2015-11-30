Shader "DT/Basic/EmitTexture"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white"{}
		_BumpMap("BumpMap", 2D) = "bump"{}
		_SpecMap("SpecularMap", 2D) = "black"{}
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1.0)
		_SpecPower("Specular Power", Range(0,1)) = 0.5
		_EmitMap("Emissive", 2D) = "black"{}
		_EmitPower("Emit Power", Range(0, 1.5)) = 0.5
		_EmitMap2("Emissive2", 2D) = "black"{}
		_EmitPower2("Emit Power2", Range(0, 1.5)) = 0.5
		
	}
	
	SubShader
	{
		Tags {"RenderType" = "Opague"}
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma exclude_renderers flash
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _SpecMap;
		float _SpecPower;
		sampler2D _EmitMap;
		float _EmitPower;
		sampler2D _EmitMap2;
		float _EmitPower2;
		
		struct Input
		{
			float2 uv_MainTex; //(1.0, 1.0)
			float2 uv_BumpMap;
			float2 uv_SpecMap;
			float2 uv_EmitMap;
			float2 uv_EmitMap2;
			//float4 color : COLOR; //(1.0, 1.0, 1.0, 1.0) R, G, B, A
		};
			
		void surf(Input IN, inout SurfaceOutput o)
		{
			//o.Albedo = 1;
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 specTex = tex2D(_SpecMap, IN.uv_SpecMap);
			fixed4 emitTex = tex2D(_EmitMap, IN.uv_EmitMap);
			fixed4 emitTex2 = tex2D(_EmitMap2, IN.uv_EmitMap2);
			
			
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Specular = _SpecPower;
			o.Gloss = specTex.rgb;
			o.Emission = (emitTex.rgb * _EmitPower);
			o.Emission += (emitTex2.rgb * _EmitPower2);
			
		}
		ENDCG
	}
	Fallback "Specular"
}