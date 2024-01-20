using FixMath;
using Microsoft.Xna.Framework;

namespace ArcherEngine.Core;

public static class F32Vec2Extensions
{
	public static Vector2 ToVector2(this F32Vec2 value)
		=> new(value.X.Float, value.Y.Float);
}