#ifndef CELLIB_INCLUDED
#define CELLIB_INCLUDED

#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _Normal;
uniform sampler2D _Palette;

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
	float3 wNorm : NORMAL;
	half3 uv : TEXCOORD0;
	float3 wPos : TEXCOORD1;
	float3 tangDL : TEXCOORD2;
	float3 tangPL : TEXCOORD3;
};

VO vert(VI vi)
{
	VO vo;
	vo.pos = UnityObjectToClipPos(vi.pos.xyz);
	vo.wNorm = UnityObjectToWorldNormal(vi.norm);
	vo.uv = vi.uv;
	vo.wPos = mul(unity_ObjectToWorld, vi.pos);
	float3 wTang = UnityObjectToWorldNormal(vi.tang);
	float3 wBiTan = cross(wTang, vo.wNorm);
	vo.tangDL = float3(dot(wTang, _WorldSpaceLightPos0), dot(wBiTan, _WorldSpaceLightPos0), dot(vo.wNorm, _WorldSpaceLightPos0));
	vo.tangPL = -float3(dot(wTang, unity_4LightPosX0[0]), dot(wBiTan, unity_4LightPosY0[0]), dot(vo.wNorm, unity_4LightPosZ0[0]));

	return vo;
}

fixed4 frag(VO i) : SV_Target
{
	half3 tNorm = UnpackNormal(tex2D(_Normal, i.uv));
	tNorm.z = -tNorm.z;
	half3 norm = normalize(tNorm);
	half3 dNorm = dot(norm, normalize(i.tangDL).xyz);
	fixed4 band = tex2D(_Palette, fixed2((dNorm.z + 1) * 0.5, i.uv.y));
	fixed4 tex = tex2D(_MainTex, i.uv.xy);
	fixed4 pixel;

#if defined (_FLOOR_ALPHA)
	tex.a = floor(tex.a);
#endif

#if defined (_ROUND_ALPHA)
	tex.a = round(tex.a);
#endif

#if defined (_CEIL_ALPHA)
	tex.a = ceil(tex.a);
#endif

	//if (tex.a == 0)
	{
		pixel = band;
	}
	
	//else
	{
		//pixel = tex;
	}

#if defined(_POINT_LIGHT)
	float3 vertLight = float3(0.0, 0.0, 0.0);

	for (int index = 0; index < 4; index++)
	{
		float3 lightDir = i.tangPL;
		float sqrDist = dot(i.tangPL, i.tangPL);
		float atten = unity_4LightAtten0[index];
		half3 pNorm = max(0.0, dot(normalize(norm), lightDir));
		float3 diffRef = atten * (1 / (atten * sqrDist)) * unity_LightColor[index].rgb * pNorm;
		
		vertLight += diffRef;
	}
	
	pixel += fixed4(vertLight, 0.0);
#endif

	return pixel;
}

#endif