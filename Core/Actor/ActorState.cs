using FixMath;
using Microsoft.Xna.Framework;

namespace ArcherEngine.Core;

public struct HitDef
{

}

public struct ActorStateFrame
{
	public F32Vec2? SetVelocity;
	public F32Vec2? AddVelocity;
	public F32? SetTraction;
	public F32? SetGravity;

	public int? SetState;
	public StateCat[] SetStateCats;

	public int? SetSprite;
	public Rectangle[] SetHurtboxes;
	public Rectangle[] SetHitboxes;
	public HitDef? SetHitDef;

	public bool? SetGrazing;
	public bool? SetInvulnerable;
	public bool? SetArmored;
	public bool? SetTurnable;
}

[Flags]
public enum ActorInputs
{
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

	public int Priority;
	public ActorInputs Input;
}