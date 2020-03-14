Shader "Unlit/MyShader"{
    Properties{
        _Color("Color Tint", Color) = (1.0, 1.0, 1.0, 1.0)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader{
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed3 _Color;

            struct a2v{
                float4 position: POSITION;
                float3 normal: NORMAL;
                float4 texcoord: TEXCOORD0;
            };

            struct v2f{
                float4 position: SV_POSITION;
                float3 color: COLOR0;
            };

            v2f vert(a2v v){
                v2f o;
                o.position = UnityObjectToClipPos(v.position);
                o.color = v.normal * 0.5 + fixed3(0.5, 0.5, 0.5);
                return o;
            }  

            fixed4 frag(v2f i) : SV_Target{
                fixed3 c = i.color;
                c *= _Color.rgb;
                return fixed4(c, 1.0);
            }
            ENDCG
        }
    }
}
