using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcherEngine.Core.GUI;

public class SpriteObject : GUIObject
{
	public string Resource = "";

	[ContentSerializerIgnore]
	public Texture2D? LoadedResource = null;
	
	// TODO: Remove these optionals once we make the editor.
	[ContentSerializer(Optional = true)]
	public IGUIObjectProperty<float> Rotation = new GUIStaticProperty<float>() { Value = 0.0f };

	[ContentSerializer(Optional = true)]
	public IGUIObjectProperty<Vector2> Origin = new GUIStaticProperty<Vector2>() { Value = Vector2.Zero };

	[ContentSerializer(Optional = true)]
	public IGUIObjectProperty<Rectangle?> SrcRect = new GUIStaticProperty<Rectangle?>() { Value = null };

	[ContentSerializer(Optional = true)]
	public IGUIObjectProperty<Color> SpriteColor = new GUIStaticProperty<Color>() { Value = Color.White };

	[ContentSerializer(Optional = true)]
	public IGUIObjectProperty<SpriteEffects> Effects = new GUIStaticProperty<SpriteEffects>() { Value = SpriteEffects.None };

	[ContentSerializer(Optional = true)]
	public IGUIObjectProperty<float> SpriteDepth = new GUIStaticProperty<float>() { Value = 0.0f };

	public override void LoadObjectResource(ContentManager content)
	{
		LoadedResource = content.Load<Texture2D>(Resource);
	}

	public override void DrawObject(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(
			texture: LoadedResource,
			position: Position + PositionOffset.GetValue(),
			sourceRectangle: SrcRect.GetValue(),
			color: SpriteColor.GetValue(),
			rotation: Rotation.GetValue(),
			origin: Origin.GetValue(),
			scale: 1.0f,
			effects: Effects.GetValue(),
			layerDepth: SpriteDepth.GetValue()
		);
	}
}

public class TextObject : SpriteObject
{
	[ContentSerializerIgnore]
	public new SpriteFont? LoadedResource = null;

	public IGUIObjectProperty<string> Text = new GUIStaticProperty<string>() { Value = "" };

	public override void LoadObjectResource(ContentManager content)
	{
		LoadedResource = content.Load<SpriteFont>(Resource);
	}

	public override void DrawObject(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawString(
			spriteFont: LoadedResource,
			text: Text.GetValue(),
			position: Position + PositionOffset.GetValue(),
			color: SpriteColor.GetValue(),
			rotation: Rotation.GetValue(),
			origin: Origin.GetValue(),
			scale: 1.0f,
			effects: Effects.GetValue(),
			layerDepth: SpriteDepth.GetValue()
		);
	}
}