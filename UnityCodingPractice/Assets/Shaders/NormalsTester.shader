Shader "NormalsTester"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float3 wPos : TEXCOORD0;
				float3 normal : TEXCOORD1;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.wPos = mul(UNITY_MATRIX_M, float4(v.vertex.xyz, 1));
				o.normal = mul( (float3x3)UNITY_MATRIX_M , v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 dirToCam = normalize(_WorldSpaceCameraPos.xyzx - i.wPos);

				//debugs normal direction
				return float4(i.normal.xyz,1);

                //return float4(1,0,0,0.5);
            }
            ENDCG
        }
    }
}
