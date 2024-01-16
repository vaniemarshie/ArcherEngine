using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcherEngine.Core.GUI;

[Serializable]
public class GUIObjectState
{
	/// <summary>
	/// Current animation from the PlayAnimation function, will run the object's default if null.
	/// </summary>
	public string? CurrentAnim = null;

	/// <summary>
	/// Time elapsed in the current animation in frames, including the object's default.
	/// </summary>
	public int TimeElapsed = 0;

	public Dictionary<string, bool> BoolVariables = new();
	public Dictionary<string, int> IntVariables = new();
}

public class GUIObject
{
	[ContentSerializerIgnore]
	public GUIObjectState State = new();
	// TODO: Insert animations definition here.

	// TODO: Remove these optionals once we make the editor.
	[ContentSerializer(Optional = true)]
	public IGUIObjectProperty<bool> Hidden = new GUIStaticProperty<bool>() { Value = false };

	[ContentSerializer(Optional = true)]
	public Vector2 Position = Vector2.Zero;

	[ContentSerializer(Optional = true)]
	public IGUIObjectProperty<Vector2> PositionOffset = new GUIStaticProperty<Vector2>() { Value = Vector2.Zero };

	public virtual void LoadObjectResource(ContentManager content) {}
	public virtual void DrawObject(SpriteBatch spriteBatch) {}
}