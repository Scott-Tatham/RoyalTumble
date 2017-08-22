Shader "Tumble/Cell"
{
	Properties
	{
		_MainTex("Detail", 2D) = "" {}
		_Palette("Palette", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Lightmode" = "ForwardBase"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
			ZWrite Off
			ZTest Off
			AlphaTest NotEqual 0.0
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Shaders/Includes/CellLib.cginc"
			
			ENDCG
		}
	}
}