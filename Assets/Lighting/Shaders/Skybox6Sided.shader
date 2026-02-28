Shader "Skybox/NightDay 6 Sided"
{
    Properties
    {
        [Header(Skybox Set 1)]
        _FrontTex1("Front (+Z)", 2D) = "white" {}
        _BackTex1("Back (-Z)", 2D) = "white" {}
        _LeftTex1("Left (+X)", 2D) = "white" {}
        _RightTex1("Right (-X)", 2D) = "white" {}
        _UpTex1("Up (+Y)", 2D) = "white" {}
        _DownTex1("Down (-Y)", 2D) = "white" {}

        [Header(Skybox Set 2)]
        _FrontTex2("Front (+Z)", 2D) = "white" {}
        _BackTex2("Back (-Z)", 2D) = "white" {}
        _LeftTex2("Left (+X)", 2D) = "white" {}
        _RightTex2("Right (-X)", 2D) = "white" {}
        _UpTex2("Up (+Y)", 2D) = "white" {}
        _DownTex2("Down (-Y)", 2D) = "white" {}

        [Header(Blending)]
        _Blend("Blend", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox"
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float3 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Texture Set 1
            sampler2D _FrontTex1;
            sampler2D _BackTex1;
            sampler2D _LeftTex1;
            sampler2D _RightTex1;
            sampler2D _UpTex1;
            sampler2D _DownTex1;

            // Texture Set 2
            sampler2D _FrontTex2;
            sampler2D _BackTex2;
            sampler2D _LeftTex2;
            sampler2D _RightTex2;
            sampler2D _UpTex2;
            sampler2D _DownTex2;

            float _Blend;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 dir = normalize(i.texcoord);
                float3 absDir = abs(dir);

                fixed4 color1;
                fixed4 color2;
                float2 uv;

                // Pick the major axis to determine which face to sample
                if(absDir.z > absDir.x && absDir.z > absDir.y)
                {
                    // Front or Back face
                    if(dir.z > 0)
                    {
                        uv = float2(dir.x, dir.y) / absDir.z * 0.5 + 0.5;
                        color1 = tex2D(_FrontTex1, uv);
                        color2 = tex2D(_FrontTex2, uv);
                    }
                    else
                    {
                        uv = float2(-dir.x, dir.y) / absDir.z * 0.5 + 0.5;
                        color1 = tex2D(_BackTex1, uv);
                        color2 = tex2D(_BackTex2, uv);
                    }
                }
                else if(absDir.x > absDir.y)
                {
                    // Left or Right face
                    if(dir.x > 0)
                    {
                        uv = float2(-dir.z, dir.y) / absDir.x * 0.5 + 0.5;
                        color1 = tex2D(_LeftTex1, uv);
                        color2 = tex2D(_LeftTex2, uv);
                    }
                    else
                    {
                        uv = float2(dir.z, dir.y) / absDir.x * 0.5 + 0.5;
                        color1 = tex2D(_RightTex1, uv);
                        color2 = tex2D(_RightTex2, uv);
                    }
                }
                else
                {
                    // Up or Down face
                    if(dir.y > 0)
                    {
                        uv = float2(dir.x, -dir.z) / absDir.y * 0.5 + 0.5;
                        color1 = tex2D(_UpTex1, uv);
                        color2 = tex2D(_UpTex2, uv);
                    }
                    else
                    {
                        uv = float2(dir.x, dir.z) / absDir.y * 0.5 + 0.5;
                        color1 = tex2D(_DownTex1, uv);
                        color2 = tex2D(_DownTex2, uv);
                    }
                }

                // Blend the colors from the two texture sets
                return lerp(color1, color2, _Blend);
            }
            ENDCG
        }
    }
}