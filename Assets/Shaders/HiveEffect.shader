// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Xin/TransitionEffect"
{
	Properties
	{
		_MainTex ("_MainTexture", 2D) = "white" {}
		_TransTex("TransTex",2D) = "white" {}
		_Cutoff("Alpha Cutoff",Range(0,1)) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite On ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _TransTex;
			fixed _Cutoff;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 transCol = tex2D(_TransTex, i.uv);
				
				//Alpha Test
				clip( -0.1 + _Cutoff - transCol.r );
				//Equal to 
				// if((texcoord.a - _Cutoff)<0.0){ discard;}

				return col;
			}
			ENDCG
		}
	}
}
