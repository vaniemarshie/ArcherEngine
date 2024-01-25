using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcherEngine.Core;

public struct CharacterSpriteSet
{
	public Dictionary<int, Rectangle> Sprites;

	[ContentSerializerIgnore]
	public Texture2D LoadedResource;
	[ContentSerializerIgnore]
	public Texture2D LoadedPaletteResource;
}

public struct Character
{
	public string InternalName;
	public string Name;

	[ContentSerializer(FlattenContent = true)]
	public CharacterSpriteSet Sprites;

	public Dictionary<int, ActorState> States;
	public Dictionary<int, int[]> StateCats;
}

public static class CharacterExtensions
{
	public static void LoadContent(this ref Character _char, ContentManager content)
	{
		// Check if the textures haven't already been loaded.
		if (_char.Sprites.LoadedResource == null || _char.Sprites.LoadedPaletteResource == null)
		{
			_char.Sprites.LoadedResource = content.Load<Texture2D>("Sprites/Characters/" + _char.InternalName);
			_char.Sprites.LoadedPaletteResource = content.Load<Texture2D>("Sprites/Characters/" + _char.InternalName + "_pal");
		}
	}
}