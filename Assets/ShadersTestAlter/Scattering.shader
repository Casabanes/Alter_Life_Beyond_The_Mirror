// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Scattering"
{
	Properties
	{
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 temp_cast_0 = (i.uv_texcoord.y).xxx;
			o.Albedo = temp_cast_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
53;233;1352;677;1484.171;860.8195;1.924541;True;False
Node;AmplifyShaderEditor.RangedFloatNode;25;-744.6726,-171.7859;Inherit;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;False;-0.34;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-967.7974,400.0911;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-752.0349,-388.8394;Inherit;True;Property;_GradientHojas;GradientHojas;4;0;Create;True;0;0;False;0;False;-1;b1ab886cea0442e42a7c856acd62a1c7;b1ab886cea0442e42a7c856acd62a1c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-322.4075,-340.6919;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-395.0074,-96.22263;Inherit;False;Property;_Float5;Float 5;6;0;Create;True;0;0;False;0;False;0.46;0.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-24.59931,-268.0918;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;32;324.0655,-436.1964;Inherit;False;Property;_Color1;Color 1;7;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.9741967,0.5896226,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;31;330.7435,-618.4245;Inherit;False;Property;_Color0;Color 0;5;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;15;-865.127,40.77872;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TransformPositionNode;34;306.3302,-248.8706;Inherit;True;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;30;786.2343,-399.7028;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-225.5216,-635.6482;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1137.705,-16.72855;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformDirectionNode;8;-413.0594,464.5161;Inherit;False;Tangent;Object;True;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCRemapNode;1;-680.3768,472.2679;Inherit;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,0.5;False;3;FLOAT2;0,0;False;4;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1073.615,228.6227;Inherit;False;Constant;_Float3;Float 3;4;0;Create;True;0;0;False;0;False;3.15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-888.5403,516.4006;Inherit;False;Property;_Float2;Float 2;2;0;Create;True;0;0;False;0;False;0;-3.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;18;-1071.615,102.6227;Inherit;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-201.1542,481.4373;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;35;-318.5344,141.4366;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;33;-682.6984,-598.2374;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;6;-354.8539,613.0034;Inherit;False;Property;_Float1;Float 1;0;0;Create;True;0;0;False;0;False;0;6.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-687.1977,12.25447;Inherit;True;Property;_AlphaHoja1;AlphaHoja 1;3;0;Create;True;0;0;False;0;False;-1;62a2de41c4573634c83830adf5f93507;62a2de41c4573634c83830adf5f93507;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-911.9684,590.5091;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;0.25;-0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;20;881.7449,-171.6969;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Scattering;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;24.2;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;22;0
WireConnection;24;1;25;0
WireConnection;29;0;24;0
WireConnection;29;1;27;0
WireConnection;15;0;16;0
WireConnection;15;1;18;0
WireConnection;15;2;17;0
WireConnection;30;0;31;0
WireConnection;30;1;32;0
WireConnection;30;2;29;0
WireConnection;8;0;1;0
WireConnection;1;0;4;0
WireConnection;1;3;2;0
WireConnection;7;0;8;0
WireConnection;7;1;6;0
WireConnection;35;0;14;0
WireConnection;14;1;15;0
WireConnection;20;0;36;2
ASEEND*/
//CHKSM=80F612FC0BB6F292CF1F774383F5291648B65A41