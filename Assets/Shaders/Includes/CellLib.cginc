#ifndef CELLLIB_INCLUDED
#define CELLLIB_INCLUDED

#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _Palette;
uniform float4 _LightColor0;

struct VI
{
	float4 pos : POSITION;
	float3 norm : NORMAL;
	float4 tang : TANGENT;
	float4 uv : TEXCOORD0;
};

struct VO
{
	float4 pos : SV_POSITION;
	half3 uv : TEXCOORD0;
	float3 wPos : TEXCOORD1;
	float3 wNorm : TEXCOORD2;
};

VO vert(VI vi)
{
	VO vo;
	vo.pos = UnityObjectToClipPos(vi.pos.xyz);
	vo.uv = vi.uv;
	vo.wPos = mul(unity_ObjectToWorld, vi.pos);
	vo.wNorm = UnityObjectToWorldNormal(vi.norm);
	float3 wTang = UnityObjectToWorldNormal(vi.tang);
	float3 wBiTan = cross(wTang, vo.wNorm);

	return vo;
}

fixed4 frag(VO i) : SV_Target
{
	half3 norm = dot(i.wNorm, normalize(_WorldSpaceLightPos0).xyz);
	fixed3 band = tex2D(_Palette, fixed2((-norm + fixed3(0.5, 0.5, 0.5)).z, i.uv.y));
	fixed4 tex = tex2D(_MainTex, i.uv.xy);
	fixed4 pixel;
	tex.a = floor(tex.a);

	if (tex.a != 1)
	{
		pixel = fixed4(band, 1);
	}
	
	else
	{
		pixel = tex;
	}
	
	return pixel;
}

#endif