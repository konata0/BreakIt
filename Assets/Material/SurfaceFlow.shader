Shader "Unlit/SurfaceFlow"{
    Properties{
        _Surface ("Surface Texture", 2D) = "gray" {}
        _Flow("Flow Texture", 2D) = "black"{}
        _Route0("Route0(x, y, z, angle)", Vector) = (1, 1, -1, 45)
        _Route1("Route1(x, y, z, angle)", Vector) = (1, 1, -1, 90)
        _Route2("Route2(x, y, z, angle)", Vector) = (1, 1, -1, 135)
        _Bump("Bump", Float) = 0
        _Speed("Speed", Float) = 1
        _Diffuse("Diffuse", Color) = (1.0, 1.0, 1.0, 1.0)
        _Specular("Specular", Color) = (1.0, 1.0, 1.0, 1.0)
        _Gloss("Gloss", Range(1.0, 512)) = 10
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
            float _Bump;
            fixed4 _Diffuse;
            fixed4 _Specular;
            float _Gloss;


            struct appdata{
                float4 vertex : POSITION;
                float3 normal: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float4 uv_u1v1 : TEXCOORD1;
                float4 u2v2_u3v3 : TEXCOORD2;
                float4 pos: SV_POSITION;
                float3 worldNormal: TEXCOORD3;
                float3 worldViewDir: TEXCOORD4;

                float test:TEXCOORD5;
            };

            v2f vert (appdata v){

                v2f o;

                float PI = 3.141592653589;
                float2 flowUv = float2(0, 0);
                // 设置一条平行于旋转平面的轴
                float3 routeZ = normalize(_Route0.xyz);
                int flag = step(0.7, dot(float3(1.0, 0, 0), routeZ));
                float3 routeX = float3(1.0, 0, 0) * (1 - flag) + float3(0, 1.0, 0) * flag;
                routeX = normalize(cross(routeZ, routeX));
                // 另一条平行于旋转平面的轴
                float3 routeY = normalize(cross(routeZ, routeX));
                float3 rotateWorldNormal = normalize(mul(float3x3(routeX, routeY, routeZ), v.normal));
                // 计算该点在FLOW上的UV
                flowUv.x = 1.0 - acos(rotateWorldNormal.z) / PI;
                flag = step(0.01, dot(rotateWorldNormal.xy, rotateWorldNormal.xy));
                rotateWorldNormal.xy = normalize(rotateWorldNormal.xy * flag + float2(1.0, 1.0) * (1.0 - flag));
                flag = step(0, rotateWorldNormal.y);
                flowUv.y = acos(rotateWorldNormal.x) * (flag - 0.5) / PI + 0.5;               
                // 写入
                o.uv_u1v1 = float4(TRANSFORM_TEX(v.uv, _Surface), TRANSFORM_TEX(flowUv, _Flow));

                // 第二个
                routeZ = normalize(_Route1.xyz);
                flag = step(0.7, dot(float3(1.0, 0, 0), routeZ));
                routeX = float3(1.0, 0, 0) * (1 - flag) + float3(0, 1.0, 0) * flag;
                routeX = normalize(cross(routeZ, routeX));
                routeY = normalize(cross(routeZ, routeX));
                rotateWorldNormal = normalize(mul(float3x3(routeX, routeY, routeZ), v.normal));
                flowUv.x = 1.0 - acos(rotateWorldNormal.z) / PI;
                flag = step(0.01, dot(rotateWorldNormal.xy, rotateWorldNormal.xy));
                rotateWorldNormal.xy = normalize(rotateWorldNormal.xy * flag + float2(1.0, 1.0) * (1.0 - flag));
                flag = step(0, rotateWorldNormal.y);
                flowUv.y = acos(rotateWorldNormal.x) * (flag - 0.5) / PI + 0.5;  
                o.u2v2_u3v3.xy = TRANSFORM_TEX(flowUv, _Flow);

                // 第三个
                routeZ = normalize(_Route2.xyz);
                flag = step(0.7, dot(float3(1.0, 0, 0), routeZ));
                routeX = float3(1.0, 0, 0) * (1 - flag) + float3(0, 1.0, 0) * flag;
                routeX = normalize(cross(routeZ, routeX));
                routeY = normalize(cross(routeZ, routeX));
                rotateWorldNormal = normalize(mul(float3x3(routeX, routeY, routeZ), v.normal));
                flowUv.x = 1.0 - acos(rotateWorldNormal.z) / PI;
                flag = step(0.01, dot(rotateWorldNormal.xy, rotateWorldNormal.xy));
                rotateWorldNormal.xy = normalize(rotateWorldNormal.xy * flag + float2(1.0, 1.0) * (1.0 - flag));
                flag = step(0, rotateWorldNormal.y);
                flowUv.y = acos(rotateWorldNormal.x) * (flag - 0.5) / PI + 0.5;  
                o.u2v2_u3v3.zw = TRANSFORM_TEX(flowUv, _Flow);

                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                o.worldViewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);

                return o;

            }

            fixed4 frag (v2f o) : SV_Target{
                // 表面纹理颜色
                fixed3 surfaceColor = tex2D(_Surface, o.uv_u1v1.xy).rgb;
                // 表面流动
                fixed3 flowColor = tex2D(_Flow, o.uv_u1v1.zw).rgb;
                //flowColor += tex2D(_Flow, o.u2v2_u3v3.xy).rgb;
                //flowColor += tex2D(_Flow, o.u2v2_u3v3.zw).rgb;
                // 环境光
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                // 漫反射
                fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(o.worldNormal, worldLight));
                // 镜面反射高光
                fixed3 halfDir = normalize(worldLight + o.worldViewDir);
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(o.worldNormal, halfDir)), _Gloss);
                // 最终颜色
                fixed3 color = (ambient + diffuse + specular) * surfaceColor + flowColor;

                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
