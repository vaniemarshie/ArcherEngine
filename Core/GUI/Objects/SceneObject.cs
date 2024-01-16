using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcherEngine.Core.GUI;

public class SceneObject : GUIObject
{
	public string Scene = "";
	private Scene? _scene;

	public override void LoadObjectResource(ContentManager content)
	{
		_scene = content.Load<Scene>("Scenes/" + Scene);

		foreach(GUIObject obj in _scene.Objects)
		{
			obj.Position += Position;
		}

		_scene.LoadSceneAssets(content);
	}

	public override void DrawObject(SpriteBatch spriteBatch)
	{
		_scene.DrawScene(spriteBatch);
	}
}