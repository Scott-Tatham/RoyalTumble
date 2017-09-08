Shader "Tumble/Refraction"
{
	Properties
	{
		_RefIndex("Refractive Index", float) = 0
		_Cube("Reflection Map", Cube) = "" {}
	}

	SubShader
	{
		Tags
		{
			//"Lightmode" = "ForwardBase"
			//"IgnoreProjector" = "True"
			//"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
			//ZWrite Off
			//ZTest Off
			//AlphaTest NotEqual 0.0
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Shaders/Includes/EffectLib.cginc"
	
			ENDCG
		}
	}
}