using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcherEngine.Core;

public struct CharacterSpriteSet
{
	public string Resource;
	public Texture2D? LoadedResource;
	public Dictionary<int, Rectangle> Sprites;
}

public struct Character
{
	public string Name;
	public Dictionary<int, ActorState> States;
	public Dictionary<int, int[]> StateCats;
	public CharacterSpriteSet Sprites;
}