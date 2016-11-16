Shader "Custom/HatchingOpaque" {
	Properties{
		_Hatch0("Hatch 0", 2D) = "white" {}
	_Hatch1("Hatch 1", 2D) = "white" {}
	_Hatch2("Hatch 2", 2D) = "white" {}
	_Hatch3("Hatch 3", 2D) = "white" {}
	_Hatch4("Hatch 4", 2D) = "white" {}
	_Hatch5("Hatch 5", 2D) = "white" {}
	_Color("Color", Color) = (1, 1, 1, 1)
	//_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque"  } // "IgnoreProjector" = "True" }
		//Cull Back
		//ZWrite Off
		//ColorMask 0
		//UsePass "Transparent/Diffuse/FORWARD"
		//ZTest Always
		//Blend One OneMinusSrcAlpha
		//Blend SrcAlpha One
		CGPROGRAM
#pragma target 3.0
#pragma surface surf SimpleLambert

		sampler2D _Hatch0;
	sampler2D _Hatch1;
	sampler2D _Hatch2;
	sampler2D _Hatch3;
	sampler2D _Hatch4;
	sampler2D _Hatch5;
	//sampler2D _MainTex;
	float4 _Color;
	float4 c = Vector(1, 1, 1, 1);

	

	half4 LightingSimpleLambert(inout SurfaceOutput s, half3 lightDir, half atten)
	{
		float2 uv = s.Albedo.xy *2;
		s.Albedo = 0.0;


		half NdotL = dot(s.Normal, lightDir);
		half diff = NdotL * 0.75 + 0.25;
		

		half lightColor = _LightColor0.r * 0.3 + _LightColor0.g * 0.59 + _LightColor0.b * 0.11;
		half intensity = lightColor * (diff * atten * 2);
		intensity = saturate(intensity);

		float part = 1 / 6.0;
		// KLUDGE: with "if" instead of "else if" lines between regions are invisible		
		if (intensity <= part)
		{
			float temp = intensity;
			temp *= 6;
			c.rgb = lerp(tex2D(_Hatch0, uv), tex2D(_Hatch1, uv), temp);
		}
		if (intensity > part && intensity <= part * 2)
		{
			float temp = intensity - part / 100;
			temp *= 6;
			c.rgb = lerp(tex2D(_Hatch1, uv), tex2D(_Hatch2, uv), temp);
		}
		if (intensity > part * 2 && intensity <= part * 3)
		{
			float temp = intensity - part * 2 / 100;
			temp *= 6;
			c.rgb = lerp(tex2D(_Hatch2, uv), tex2D(_Hatch3, uv), temp);
		}
		if (intensity > part * 3 && intensity <= part * 4)
		{
			float temp = intensity - part * 3 / 100;
			temp *= 6;
			c.rgb = lerp(tex2D(_Hatch3, uv), tex2D(_Hatch4, uv), temp);
		}
		if (intensity > part * 4 && intensity <= part * 5)
		{
			float temp = intensity - part * 4 / 100;
			temp *= 6;
			c.rgb = lerp(tex2D(_Hatch4, uv), tex2D(_Hatch5, uv), temp);
		}
		if (intensity > part * 5)
		{
			float temp = intensity - part * 5 / 100;
			temp *= 6;
			c.rgb = lerp(tex2D(_Hatch5, uv), 1, temp);
		}
		c.rgb = c.rgb * _Color.rgb;		
		c.a = 1.0;

		//float maxLight = 1.0f;
		//if (c.r > maxLight) c.r = maxLight;
		//if (c.g > maxLight) c.g = maxLight;
		//if (c.b > maxLight) c.b = maxLight;
		//if (c.a > maxLight) c.a = maxLight;

		return c;
	}

	struct Input
	{
		float2 uv_Hatch0;
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		o.Albedo.xy = IN.uv_Hatch0;
		o.Alpha = _Color.a;
	}
	ENDCG
	}
		Fallback "Diffuse"
}