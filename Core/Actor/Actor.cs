using FixMath;
using Microsoft.Xna.Framework;

namespace ArcherEngine.Core;
public struct StateCat
{
	public int Category;
	public bool OnlyOnHit;
}

[Serializable]
public class Actor
{
	public bool Active;

	public (SoulInput[] buffer, int latest) InputBuffer = (new SoulInput[Constants.MaxInputBuffer], 0);

	public int? Health;
	public int? WhiteHealth;

	public F32Vec2 Position;
	public F32Vec2 Velocity;
	public F32 Traction;
	public F32 Gravity;

	public bool IsFlipped;

	public int State;
	public int StateTime;
	public bool IsOnFloor;
	public StateCat[] StateCats = Array.Empty<StateCat>();

	public int Sprite;
	public Rectangle[] Hurtboxes = Array.Empty<Rectangle>();
	public Rectangle[] Hitboxes = Array.Empty<Rectangle>();
	public HitDef HitDefinition;
	public bool HasHit;

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

	/// <summary>
	/// Appends the latest input to the input buffer.
	/// </summary>
	void GetActorInput()
	{
		SoulInput input = Soul.GetInput();
		
		// Increase the latest number, modulating by how large the buffer is.
		InputBuffer.latest = (InputBuffer.latest + 1) % Constants.MaxInputBuffer;

		// Set the latest input.
		InputBuffer.buffer[InputBuffer.latest] = input;
	}

	ActorInputs GetStateInputs()
	{
		// The SoulInput is the same as the Hold Inputs on an ActorInput so we can just carry them over.
		ActorInputs inputs = (ActorInputs)InputBuffer.buffer[InputBuffer.latest];

		// i is set to 1 so we always skip the most recent input.
		for (int i = 1; i < Constants.MaxInputBuffer; i++)
		{
			int thisFrame = (int)InputBuffer.buffer[(InputBuffer.latest + i) % Constants.MaxInputBuffer];
			int nextFrame = (int)InputBuffer.buffer[(InputBuffer.latest + i - 1) % Constants.MaxInputBuffer];

			// Mask out each button that was pressed on the next frame.
			inputs |= (ActorInputs)((int)ActorInputs.TapButtons << (~thisFrame & nextFrame));
		}

		return inputs;
	}

	public void Update()
	{
		GetActorInput();
		ActorInputs input = GetStateInputs();

		int highestPriority = 0;

		// Check each category that can be canceled into if the input is correct.
		foreach (StateCat cat in StateCats)
		{
			if (cat.OnlyOnHit && !HasHit) continue;
			foreach (int stateIndex in Char.StateCats[cat.Category])
			{
				ActorState state = Char.States[stateIndex];

				if ((state.Input & (IsFlipped ? input.Flip() : input)) != 0 && state.Priority > highestPriority)
				{
					State = stateIndex;
					StateTime = 0;
					HasHit = false;
					highestPriority = state.Priority;
				}
			}
		}

		ActorState currentState = Char.States[State];

		// Try and get the current frame of the state if it exists.
		if (currentState.Frames.TryGetValue(StateTime, out ActorStateFrame frame))
		{
			if (frame.SetVelocity.HasValue) Velocity = frame.SetVelocity.Value * (IsFlipped ? F32Vec2.FromInt(-1, 1) : F32Vec2.FromInt(1, 1));
			if (frame.AddVelocity.HasValue) Velocity += frame.AddVelocity.Value * (IsFlipped ? F32Vec2.FromInt(-1, 1) : F32Vec2.FromInt(1, 1));
			if (frame.SetTraction.HasValue) Traction = frame.SetTraction.Value;
			if (frame.SetGravity.HasValue) Gravity = frame.SetGravity.Value;

			if (frame.SetState.HasValue) State = frame.SetState.Value;
			if (frame.SetStateCats != null) StateCats = frame.SetStateCats;

			if (frame.SetSprite.HasValue) Sprite = frame.SetSprite.Value;
			if (frame.SetHurtboxes != null) Hurtboxes = frame.SetHurtboxes;
			if (frame.SetHitboxes != null) Hitboxes = frame.SetHitboxes;
			if (frame.SetHitDef.HasValue) HitDefinition = frame.SetHitDef.Value;

			if (frame.SetGrazing.HasValue) Grazing = frame.SetGrazing.Value;
			if (frame.SetInvulnerable.HasValue) Invulnerable = frame.SetInvulnerable.Value;
			if (frame.SetArmored.HasValue) Armored = frame.SetArmored.Value;
			if (frame.SetTurnable.HasValue) Turnable = frame.SetTurnable.Value;
		}
	}
}