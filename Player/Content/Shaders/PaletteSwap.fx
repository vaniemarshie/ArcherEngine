// Taken from https://community.monogame.net/t/palette-swap-shader-issues-using-r-channel-as-an-index/17424/6

#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

extern bool active;
extern float palette;

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
	Filter = Point;
};

Texture2D PaletteTexture;
sampler2D PaletteTextureSampler = sampler_state
{
	Texture = <PaletteTexture>;
	Filter = Point;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};


float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	if (active && color.a > 0)
	{	
		color = tex2D(PaletteTextureSampler, float2(color.r, palette));
	}
	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};