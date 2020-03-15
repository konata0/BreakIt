Shader "Unlit/MyShader"{
    Properties{
        _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _MainTex("Main Texture", 2D) = "white"{}
        _BumpMap("Bump Map", 2D) = "bump"{}
        _BumpScale("Bump Scale", Float) = 1.0
        _Specular("Specular", Color) = (1.0, 1.0, 1.0, 1.0)
        _Gloss("Gloss", Range(8.0, 256)) = 20
        
    }
    SubShader{
        Pass{

            Tags{
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;
            fixed4 _Specular;
            float _Gloss;
            
            
            struct a2v{
                float4 vertex: POSITION;
                float3 normal: NORMAL;
                float4 tangent: TANGENT;
                float3 texcoord: TEXCOORD0;
            };

            struct v2f{
                float4 pos: SV_POSITION;
                float4 uv: TEXCOORD0;
                float3 lightDir: TEXCOORD1;
                float3 viewDir: TEXCOORD2;
            };
            

            v2f vert(a2v i){
                v2f v;
                v.pos = UnityObjectToClipPos(i.vertex);
                v.uv.xy = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                v.uv.zw = i.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

                float3 binormal = cross(normalize(i.normal), normalize(i.tangent.xyz)) * i.tangent.w;
                float3x3 rotation = float3x3(i.tangent.xyz, binormal, i.normal);

                v.lightDir = normalize(mul(rotation, ObjSpaceLightDir(i.vertex)).xyz);
                v.viewDir = normalize(mul(rotation, ObjSpaceViewDir(i.vertex)).xyz);
                
                return v;
            }  

            fixed4 frag(v2f v) : SV_Target{

                fixed4 packedNormal = tex2D(_BumpMap, v.uv.zw);
                fixed3 tangentNormal;
                tangentNormal.xy = (packedNormal.xy * 2 - 1) * _BumpScale;
                tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));

                fixed3 albebo = tex2D(_MainTex, v.uv).rgb * _Color.rgb;

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albebo;
                fixed3 diffuse = _LightColor0.rgb * albebo * saturate(dot(tangentNormal, v.lightDir));

                fixed3 halfDir = normalize(v.lightDir + v.viewDir);
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(tangentNormal, halfDir)), _Gloss);

                return fixed4(ambient + diffuse + specular, 1.0);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}

