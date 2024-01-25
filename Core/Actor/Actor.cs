using FixMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
	public F32 MaxXVol;

	public bool IsFlipped;

	public int State;
	public int StateTime;
	public bool IsOnFloor;
	public StateCat[] StateCats = Array.Empty<StateCat>();

	public int Sprite;
	public int DefaultPalette;
	public int Palette;
	public Rectangle[] Hurtboxes = Array.Empty<Rectangle>();
	public Rectangle[] Hitboxes = Array.Empty<Rectangle>();
	public HitDef HitDefinition;
	public bool HasHit;

	public bool Grazing;
	public bool Invulnerable;
	public bool Armored;

	readonly Character _char;
	readonly ISoul _soul;

	public Actor(Character character, ISoul soul)
	{
		_char = character;
		_soul = soul;
	}

	/// <summary>
	/// Appends the latest input to the input buffer.
	/// </summary>
	void GetActorInput()
	{
		SoulInput input = _soul.GetInput();
		
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

	public void SetState(int state)
	{
		State = state;
		StateTime = 0;
		HasHit = false;
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
			foreach (int stateIndex in _char.StateCats[cat.Category])
			{
				ActorState state = _char.States[stateIndex];

				if (((state.Input & (IsFlipped ? input.Flip() : input)) != 0 || state.Input == ActorInputs.Nothing) && state.Priority > highestPriority)
				{
					SetState(stateIndex);
					highestPriority = state.Priority;
				}
			}
		}

		ActorState currentState = _char.States[State];

		// Try and get the current frame of the state if it exists.
		if (currentState.Frames.TryGetValue(StateTime, out ActorStateFrame frame))
		{
			// Do flipping first with velocity.
			if (frame.SetFlipped.HasValue) IsFlipped = frame.SetFlipped.Value;

			if (frame.SetVelocity.HasValue) Velocity = frame.SetVelocity.Value * (IsFlipped ? F32Vec2.FromInt(-1, 1) : F32Vec2.FromInt(1, 1));
			if (frame.AddVelocity.HasValue) Velocity += frame.AddVelocity.Value * (IsFlipped ? F32Vec2.FromInt(-1, 1) : F32Vec2.FromInt(1, 1));
			if (frame.SetTraction.HasValue) Traction = frame.SetTraction.Value;
			if (frame.SetGravity.HasValue) Gravity = frame.SetGravity.Value;

			if (frame.SetState.HasValue) SetState(frame.SetState.Value);
			if (frame.SetStateCats != null) StateCats = frame.SetStateCats;

			if (frame.SetSprite.HasValue) Sprite = frame.SetSprite.Value;
			if (frame.SetPalette.HasValue) 
			{
				Palette = frame.SetPalette.Value;
				if (frame.SetPalette.Value == -1)
				{
					Palette = DefaultPalette;
				}
			}
			if (frame.SetHurtboxes != null) Hurtboxes = frame.SetHurtboxes;
			if (frame.SetHitboxes != null) Hitboxes = frame.SetHitboxes;
			if (frame.SetHitDef.HasValue) HitDefinition = frame.SetHitDef.Value;

			if (frame.SetGrazing.HasValue) Grazing = frame.SetGrazing.Value;
			if (frame.SetInvulnerable.HasValue) Invulnerable = frame.SetInvulnerable.Value;
			if (frame.SetArmored.HasValue) Armored = frame.SetArmored.Value;
		}

		StateTime = (StateTime + 1) % currentState.Length;

		// TODO: I would put a colision call here, IF I HAD ONE
	}

	public void Draw(SpriteBatch spriteBatch, Effect paletteEffect)
	{
		// TODO: Camera things, probably

		// Set palette settings
		paletteEffect.Parameters["active"].SetValue(true);
		paletteEffect.Parameters["palette"].SetValue((float)Palette / (float)Constants.PaletteCount);
		paletteEffect.Parameters["PaletteTexture"].SetValue(_char.Sprites.LoadedPaletteResource);

		// Draw!
		spriteBatch.Draw(
			texture: _char.Sprites.LoadedResource,
			position: Position.ToFlooredV2(),
			sourceRectangle: _char.Sprites.Sprites[Sprite],
			color: Color.White,
			rotation: 0f,
			origin: Vector2.Zero,
			scale: 0.0f,
			effects: IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
			layerDepth: 1.0f
		);
	}
}