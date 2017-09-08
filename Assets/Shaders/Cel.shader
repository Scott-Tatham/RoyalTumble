Shader "Tumble/Cel"
{
	Properties
	{
		_MainTex("Detail Map", 2D) = "" {}
		_Normal("Normal Map", 2D) = "bump" {}
		_Palette("Palette", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Lightmode" = "ForwardBase"
			"IgnoreProjector" = "True"
			"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
			ZWrite Off
			ZTest Off
			AlphaTest NotEqual 0.0
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM

			#pragma shader_feature _NORMAL
			#pragma shader_feature _FLOOR_ALPHA
			#pragma shader_feature _ROUND_ALPHA
			#pragma shader_feature _CEIL_ALPHA
			#pragma shader_feature _POINT_LIGHT

			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Shaders/Includes/CelLib.cginc"
			
			ENDCG
		}
	}

	CustomEditor "CelEditor"
}