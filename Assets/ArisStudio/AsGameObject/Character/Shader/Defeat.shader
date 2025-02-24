﻿Shader "AsCharcater/Defeat"
{
    Properties
    {
        _FillColor ("FillColor", Color) = (1,1,1,1)
        _Highlight ("Highlight", Range(0, 1)) = 0
        [NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
        _Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
    }
    SubShader
    {

        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane"
        }
        Blend One OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Lighting Off

        Stencil
        {
            Ref[_StencilRef]
            Comp[_StencilComp]
            Pass Keep
        }

        Pass
        {
            Name "Normal"


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _FillColor;
            float _Highlight;

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o = (VertexOutput)0;
                o.uv = v.uv;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(VertexOutput i) : SV_Target
            {
                float4 rawColor = tex2D(_MainTex, i.uv);
                float finalAlpha = (rawColor.a * i.vertexColor.a);

                rawColor.rgb *= rawColor.a;

                float3 finalColor = lerp((rawColor.rgb * i.vertexColor.rgb), (_FillColor.rgb * finalAlpha), 1 - _Highlight);
                // make sure to PMA _FillColor.
                return fixed4(finalColor, finalAlpha);
            }
            ENDCG
        }

        Pass
        {
            Name "Caster"
            Tags
            {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            ZWrite On
            ZTest LEqual

            Fog
            {
                Mode Off
            }
            Cull Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            fixed _Cutoff;

            struct VertexOutput
            {
                V2F_SHADOW_CASTER;
                float4 uvAndAlpha : TEXCOORD1;
            };

            VertexOutput vert(appdata_base v, float4 vertexColor : COLOR)
            {
                VertexOutput o;
                o.uvAndAlpha = v.texcoord;
                o.uvAndAlpha.a = vertexColor.a;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }

            float4 frag(VertexOutput i) : SV_Target
            {
                fixed4 texcol = tex2D(_MainTex, i.uvAndAlpha.xy);
                clip(texcol.a * i.uvAndAlpha.a - _Cutoff);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "SpineShaderWithOutlineGUI"
}
