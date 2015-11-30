Shader "DT/Basic/SimpleShader"
{
	SubShader
	{
		Tags {"RenderType" = "Opague"}
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input
		{
			float4 color : COLOR; //(1.0, 1.0, 1.0, 1.0) R, G, B, A
		};
		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
}