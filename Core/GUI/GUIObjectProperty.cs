using Microsoft.Xna.Framework;

namespace ArcherEngine.Core.GUI;

public interface IGUIObjectProperty<T>
{
	public T GetValue();
}

public class GUIStaticProperty<T> : IGUIObjectProperty<T>
{
	public T Value;
	public T GetValue() => Value;
}

public class GUIToggleableProperty<T> : IGUIObjectProperty<T>
{
	public T FalseValue;
	public T TrueValue;
	public int Time;
	protected bool _variable;
	protected int _framesSinceToggle;

	protected bool ShouldUseLerp() => _framesSinceToggle < Time;
	protected float GetLerpAmount() => MathF.Abs((_framesSinceToggle / Time) - (_variable ? 1.0f : 0.0f));

	public virtual void Update(bool variable)
	{
		// TODO: Find a way to make this somehow work with serialization.
		_framesSinceToggle++;
		if (_variable != variable) _framesSinceToggle = 0;

		_variable = variable;
	}

	public virtual T GetValue()
	{
		return _variable ? TrueValue : FalseValue;
	}
}

public class GUIToggleableFloat : GUIToggleableProperty<float>
{
	public override float GetValue()
	{
		if (ShouldUseLerp())
			return FalseValue * (1.0f - GetLerpAmount()) + TrueValue * GetLerpAmount();
		return base.GetValue();
	}
}

public class GUIToggleableVector2 : GUIToggleableProperty<Vector2>
{
	public override Vector2 GetValue()
	{
		if (ShouldUseLerp())
			return Vector2.Lerp(FalseValue, TrueValue, GetLerpAmount());
		return base.GetValue();
	}
}

public class GUIToggleableColor : GUIToggleableProperty<Color>
{
	public override Color GetValue()
	{
		if (ShouldUseLerp())
			return Color.Lerp(FalseValue, TrueValue, GetLerpAmount());
		return base.GetValue();
	}
}