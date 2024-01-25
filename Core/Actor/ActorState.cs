using FixMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ArcherEngine.Core;

public struct HitDef
{

}

public struct ActorStateFrame
{
	[ContentSerializer(Optional = true)]
	public F32Vec2? SetVelocity;
	[ContentSerializer(Optional = true)]
	public F32Vec2? AddVelocity;
	[ContentSerializer(Optional = true)]
	public F32? SetTraction;
	[ContentSerializer(Optional = true)]
	public F32? SetGravity;
	[ContentSerializer(Optional = true)]
	public F32? SetMaxXVol;

	[ContentSerializer(Optional = true)]
	public bool? SetFlipped;

	[ContentSerializer(Optional = true)]
	public int? SetState;
	[ContentSerializer(Optional = true)]
	public StateCat[] SetStateCats;

	[ContentSerializer(Optional = true)]
	public int? SetSprite;
	[ContentSerializer(Optional = true)]
	public int? SetPalette;
	[ContentSerializer(Optional = true)]
	public Rectangle[] SetHurtboxes;
	[ContentSerializer(Optional = true)]
	public Rectangle[] SetHitboxes;
	[ContentSerializer(Optional = true)]
	public HitDef? SetHitDef;

	[ContentSerializer(Optional = true)]
	public bool? SetGrazing;
	[ContentSerializer(Optional = true)]
	public bool? SetInvulnerable;
	[ContentSerializer(Optional = true)]
	public bool? SetArmored;
}

[Flags]
public enum ActorInputs
{
	Nothing = 0,

	HoldButtons = 1 << 0,

	HoldLight = HoldButtons << 0,
	HoldHeavy = HoldButtons << 1,
	HoldShot = HoldButtons << 2,
	HoldDash = HoldButtons << 3,
	HoldGuard = HoldButtons << 4,

	HoldUp = HoldButtons << 5,
	HoldDown = HoldButtons << 6,
	HoldFrwd = HoldButtons << 7,
	HoldBkwd = HoldButtons << 8,

	TapButtons = 1 << 16,

	TapLight = TapButtons << 0,
	TapHeavy = TapButtons << 1,
	TapShot = TapButtons << 2,
	TapDash = TapButtons << 3,
	TapGuard = TapButtons << 4,

	TapUp = TapButtons << 5,
	TapDown = TapButtons << 6,
	TapFrwd = TapButtons << 7,
	TapBkwd = TapButtons << 8,

	// TODO: Simple enums for button combinations, though this should be fairly easy.
}

public static class ActorInputsExtensions
{
	public static ActorInputs Flip(this ActorInputs input)
		=> input ^ ActorInputs.HoldFrwd | ActorInputs.HoldBkwd | ActorInputs.TapFrwd | ActorInputs.TapBkwd;
}

public struct ActorState
{
	/// <summary>
	/// Dictionary for every state frame in this state, the key is used for the frame this is meant to play on.
	/// </summary>
	public Dictionary<int, ActorStateFrame> Frames;
	public int Length;
	public ActorInputs Input;
	public bool OnFloor;
	public int Priority;
}