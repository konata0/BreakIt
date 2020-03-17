Shader "Unlit/SurfaceFlow"{
    Properties{
        _Surface ("Surface Texture", 2D) = "gray" {}
        _Flow("Flow Texture", 2D) = "black"{}
        _Route0("Route0(x, y, z, delay)", Vector) = (1, 0, 0, 45)
        _Route1("Route1(x, y, z, delay)", Vector) = (0, 1, 0, 90)
        _Route2("Route2(x, y, z, delay)", Vector) = (0, 0, 1, 135)
        _Speed("Speed", Float) = 1
        _Diffuse("Diffuse", Color) = (0.8, 0.8, 0.8, 1.0)
        _Specular("Specular", Color) = (1.0, 1.0, 1.0, 1.0)
        _FlowColor("Flow Color", Color) = (0.6, 0.6, 0.6, 1.0)
        _Gloss("Gloss", Range(1.0, 1024)) = 256
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass{
            Tags{
                "LightMode" = "ForwardBase"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _Surface;
            float4 _Surface_ST;
            sampler2D _Flow;
            float4 _Flow_ST;
            float4 _Route0;
            float4 _Route1;
            float4 _Route2;
            float _Speed;
            fixed4 _Diffuse;
            fixed4 _Specular;
            fixed4 _FlowColor;
            float _Gloss;


            struct appdata{
                float4 vertex : POSITION;
                float3 normal: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD1;
                float4 pos: SV_POSITION;
                float3 worldNormal: TEXCOORD2;
                float3 worldViewDir: TEXCOORD3;
                float3 rotateWorldNormal0: TEXCOORD4;
                float3 rotateWorldNormal1: TEXCOORD5;
                float3 rotateWorldNormal2: TEXCOORD6;
            };

            v2f vert (appdata v){

                v2f o;
                // 设置一条平行于旋转平面的轴
                float3 routeZ = normalize(_Route0.xyz);
                int flag = step(0.7, dot(float3(1.0, 0, 0), routeZ));
                float3 routeX = float3(1.0, 0, 0) * (1 - flag) + float3(0, 1.0, 0) * flag;
                routeX = normalize(cross(routeZ, routeX));
                // 另一条平行于旋转平面的轴
                float3 routeY = normalize(cross(routeZ, routeX));
                // 旋转坐标系下法线
                o.rotateWorldNormal0 = normalize(mul(float3x3(routeX, routeY, routeZ), v.normal));

                // 第二条
                routeZ = normalize(_Route1.xyz);
                flag = step(0.7, dot(float3(1.0, 0, 0), routeZ));
                routeX = float3(1.0, 0, 0) * (1 - flag) + float3(0, 1.0, 0) * flag;
                routeX = normalize(cross(routeZ, routeX));
                routeY = normalize(cross(routeZ, routeX));
                o.rotateWorldNormal1 = normalize(mul(float3x3(routeX, routeY, routeZ), v.normal));

                // 第三条
                routeZ = normalize(_Route2.xyz);
                flag = step(0.7, dot(float3(1.0, 0, 0), routeZ));
                routeX = float3(1.0, 0, 0) * (1 - flag) + float3(0, 1.0, 0) * flag;
                routeX = normalize(cross(routeZ, routeX));
                routeY = normalize(cross(routeZ, routeX));
                o.rotateWorldNormal2 = normalize(mul(float3x3(routeX, routeY, routeZ), v.normal));

                o.uv = TRANSFORM_TEX(v.uv, _Surface);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                o.worldViewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);

                return o;

            }

            fixed4 frag (v2f o) : SV_Target{
                // 表面纹理颜色
                fixed3 surfaceColor = tex2D(_Surface, o.uv).rgb;
                
                // 表面流动
                float PI = 3.141592653589;
                float3 flowColor = float3(0, 0, 0);
                float3 rotateWorldNormal = float3(0, 0, 0);
                float2 flowUv;
                float2 flowUvOffset;
                int flag;
                // 1
                rotateWorldNormal = normalize(o.rotateWorldNormal0);
                flowUv.y = 1.0 - acos(rotateWorldNormal.z) / PI;
                flag = step(0.01, dot(rotateWorldNormal.xy, rotateWorldNormal.xy));
                rotateWorldNormal.xy = normalize(rotateWorldNormal.xy * flag + float2(1.0, 1.0) * (1.0 - flag));
                flag = step(0, rotateWorldNormal.y);
                flowUv.x = acos(rotateWorldNormal.x) * (flag - 0.5) / PI + 0.5;  
                flowUvOffset = float2( _Time[1] * _Speed + _Route0.w, 0);
                flowColor += tex2D(_Flow, flowUv - flowUvOffset).rgb;
                // 2
                rotateWorldNormal = normalize(o.rotateWorldNormal1);
                flowUv.y = 1.0 - acos(rotateWorldNormal.z) / PI;
                flag = step(0.01, dot(rotateWorldNormal.xy, rotateWorldNormal.xy));
                rotateWorldNormal.xy = normalize(rotateWorldNormal.xy * flag + float2(1.0, 1.0) * (1.0 - flag));
                flag = step(0, rotateWorldNormal.y);
                flowUv.x = acos(rotateWorldNormal.x) * (flag - 0.5) / PI + 0.5;  
                flowUvOffset = float2( _Time[1] * _Speed + _Route1.w, 0);
                flowColor += tex2D(_Flow, flowUv - flowUvOffset).rgb;
                // 3
                rotateWorldNormal = normalize(o.rotateWorldNormal2);
                flowUv.y = 1.0 - acos(rotateWorldNormal.z) / PI;
                flag = step(0.01, dot(rotateWorldNormal.xy, rotateWorldNormal.xy));
                rotateWorldNormal.xy = normalize(rotateWorldNormal.xy * flag + float2(1.0, 1.0) * (1.0 - flag));
                flag = step(0, rotateWorldNormal.y);
                flowUv.x = acos(rotateWorldNormal.x) * (flag - 0.5) / PI + 0.5;  
                flowUvOffset = float2( _Time[1] * _Speed + _Route2.w, 0);
                flowColor += tex2D(_Flow, flowUv - flowUvOffset).rgb;

                // 环境光
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                // 漫反射
                fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(o.worldNormal, worldLight));
                // 镜面反射高光
                fixed3 halfDir = normalize(worldLight + o.worldViewDir);
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(o.worldNormal, halfDir)), _Gloss);
                // 最终颜色
                fixed3 color = (ambient + diffuse + specular) * surfaceColor + flowColor * _FlowColor.rgb;

                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }

    Fallback "Specular"
}
