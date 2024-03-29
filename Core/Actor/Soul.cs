using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArcherEngine.Core;

[Flags]
public enum SoulInput
{
	Light	= 1 << 0,
	Heavy	= 1 << 1,
	Shot	= 1 << 2,
	Dash	= 1 << 3,
	Guard	= 1 << 4,

	Up = 1 << 5,
	Down = 1 << 6,
	Left = 1 << 7,
	Right = 1 << 8,

	SOCDLeftRight = Left | Right,

	ButtonMask = Light | Heavy | Shot | Dash | Guard
}

public static class SoulInputExtensions
{
	public static void SOCDClean(this ref SoulInput input)
	{
		if((input & SoulInput.Up) != 0) input ^= SoulInput.Down;
		if((input & SoulInput.SOCDLeftRight) == SoulInput.SOCDLeftRight) input ^= SoulInput.SOCDLeftRight;
	}
}

/// <summary>
/// "Every Actor needs a Soul to reflect to it."
/// Gives input to an Actor class.
/// </summary>
public interface ISoul
{
	SoulInput GetInput(); // TODO: Add parameters to give data to CPU, netplay, and replay input types.
}

public struct ControllerConfig
{
	public Buttons Light;
	public Buttons Heavy;
	public Buttons Shot;
	public Buttons Dash;
	public Buttons Guard;

	// TODO: Add macro definitions
	// TODO: Maybe add button arrays instead?
}

public class LocalControllerPlayerSoul: ISoul
{
	public PlayerIndex Index;
	public ControllerConfig Config;

	public SoulInput GetInput()
	{
		GamePadState padState = GamePad.GetState(Index);
		SoulInput input = new();

		// Get directionals
		if (padState.DPad.Up == ButtonState.Pressed) input |= SoulInput.Up;
		if (padState.DPad.Down == ButtonState.Pressed) input |= SoulInput.Down;
		if (padState.DPad.Left == ButtonState.Pressed) input |= SoulInput.Left;
		if (padState.DPad.Right == ButtonState.Pressed) input |= SoulInput.Right;

		Vector2 leftThumbstick = padState.ThumbSticks.Left;
		if (leftThumbstick.Y > 0.8) input |= SoulInput.Up;
		if (leftThumbstick.Y < -0.8) input |= SoulInput.Down;
		if (leftThumbstick.X < -0.8) input |= SoulInput.Left;
		if (leftThumbstick.X > 0.8) input |= SoulInput.Right;

		// SOCD Clean
		input.SOCDClean();

		// Get Buttons
		if(padState.IsButtonDown(Config.Light)) input |= SoulInput.Light;
		if(padState.IsButtonDown(Config.Heavy)) input |= SoulInput.Heavy;
		if(padState.IsButtonDown(Config.Shot)) input |= SoulInput.Shot;
		if(padState.IsButtonDown(Config.Dash)) input |= SoulInput.Dash;
		if(padState.IsButtonDown(Config.Guard)) input |= SoulInput.Guard;

		return input;
	}
}

public struct KeyboardConfig
{
	public Keys Up;
	public Keys Down;
	public Keys Left;
	public Keys Right;

	public Keys Light;
	public Keys Heavy;
	public Keys Shot;
	public Keys Dash;
	public Keys Guard;

	// TODO: Same things in the controller config
}

public class LocalKeyboardPlayerSoul: ISoul
{
	public KeyboardConfig Config;

	public SoulInput GetInput()
	{
		KeyboardState keyboardState = Keyboard.GetState();
		SoulInput input = new();

		// Get directionals
		if (keyboardState.IsKeyDown(Config.Up)) input |= SoulInput.Up;
		if (keyboardState.IsKeyDown(Config.Down)) input |= SoulInput.Down;
		if (keyboardState.IsKeyDown(Config.Left)) input |= SoulInput.Left;
		if (keyboardState.IsKeyDown(Config.Right)) input |= SoulInput.Right;

		// SOCD Clean
		input.SOCDClean();

		// Get Buttons
		if (keyboardState.IsKeyDown(Config.Light)) input |= SoulInput.Light;
		if (keyboardState.IsKeyDown(Config.Heavy)) input |= SoulInput.Heavy;
		if (keyboardState.IsKeyDown(Config.Shot)) input |= SoulInput.Shot;
		if (keyboardState.IsKeyDown(Config.Dash)) input |= SoulInput.Dash;
		if (keyboardState.IsKeyDown(Config.Guard)) input |= SoulInput.Guard;

		return input;
	}
}