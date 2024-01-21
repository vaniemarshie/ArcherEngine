using FixMath;
using Microsoft.Xna.Framework;

namespace ArcherEngine.Core;

public struct HitDef
{

}

public struct ActorStateFrame
{
	public int? AddHealth;

	public F32Vec2? SetVelocity;
	public F32Vec2? AddVelocity;
	public F32? SetTraction;
	public F32? SetGravity;

	public int? SetState;

	public int? SetSprite;
	public Rectangle[]? SetHurtboxes;
	public Rectangle[]? SetHitboxes;
	public HitDef? SetHitDef;

	public bool? SetGrazing;
	public bool? SetInvulnerable;
	public bool? SetArmored;
	public bool? SetTurnable;
}

public struct ActorState
{
	/// <summary>
	/// Dictionary for every state frame in this state, the key is used for the frame this is meant to play on.
	/// </summary>
	public Dictionary<int, ActorStateFrame> Frames;
}