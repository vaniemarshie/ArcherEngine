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
	public CharacterSpriteSet Sprites;

	public readonly ActorState GetState(int index)
	{
		if(States.TryGetValue(index, out ActorState state)) return state;
		throw new Exception(string.Format("State {0} was not found on {1} ({2})", index, Name, this));
	}
}