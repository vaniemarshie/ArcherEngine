using FixMath;
using Microsoft.Xna.Framework;

namespace ArcherEngine.Core;

public enum Side: byte
{
	Left,
	Right
}

[Serializable]
public class Actor
{
	public bool Active;

	public int? Health;
	public int? WhiteHealth;

	public F32Vec2 Position;
	public F32Vec2 Velocity;
	public F32 Traction;
	public F32 Gravity;

	public Side side;

	public int State;
	public int StateTime;
	public bool IsOnFloor;

	public int Sprite;
	public Rectangle[]? Hurtboxes;
	public Rectangle[]? Hitboxes;
	public HitDef HitDefinition;

	public bool Grazing;
	public bool Invulnerable;
	public bool Armored;
	public bool Turnable;

	public Character Char;
	public ISoul Soul;

	public Actor(Character character, ISoul soul)
	{
		Char = character;
		Soul = soul;
	}
}