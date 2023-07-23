// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShaderTroncoChico"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MascaraOpacityTroncoChico("MascaraOpacityTroncoChico", 2D) = "white" {}
		_TroncoCorto_lambert1_BaseColor("TroncoCorto_lambert1_BaseColor", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TroncoCorto_lambert1_BaseColor;
		uniform float4 _TroncoCorto_lambert1_BaseColor_ST;
		uniform sampler2D _MascaraOpacityTroncoChico;
		uniform float4 _MascaraOpacityTroncoChico_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TroncoCorto_lambert1_BaseColor = i.uv_texcoord * _TroncoCorto_lambert1_BaseColor_ST.xy + _TroncoCorto_lambert1_BaseColor_ST.zw;
			o.Albedo = tex2D( _TroncoCorto_lambert1_BaseColor, uv_TroncoCorto_lambert1_BaseColor ).rgb;
			o.Alpha = 1;
			float2 uv_MascaraOpacityTroncoChico = i.uv_texcoord * _MascaraOpacityTroncoChico_ST.xy + _MascaraOpacityTroncoChico_ST.zw;
			clip( ( tex2D( _MascaraOpacityTroncoChico, uv_MascaraOpacityTroncoChico ) * 1.64 ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
-9;449;1352;701;965.012;59.64124;1;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-574.7954,206.1593;Inherit;True;Property;_MascaraOpacityTroncoChico;MascaraOpacityTroncoChico;1;0;Create;True;0;0;False;0;False;-1;a5d664adcb2270d429f415af34bc46b7;c08a9e6b66bffdd4b89bf752da8e0e0b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-443.6873,487.4263;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;False;1.64;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-301.6873,-70.57373;Inherit;True;Property;_TroncoCorto_lambert1_BaseColor;TroncoCorto_lambert1_BaseColor;2;0;Create;True;0;0;False;0;False;-1;484902d1a87a89846985f6764e2f4644;d416650061586734aa7b69f8501ffd7c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-256.6873,388.4263;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ShaderTroncoChico;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;5;1;6;0
WireConnection;0;0;3;0
WireConnection;0;10;5;0
ASEEND*/
//CHKSM=1F6178B9B6E6841B42682EC07FFA9EB1D2BEE1ED