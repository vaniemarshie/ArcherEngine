using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcherEngine.Core.GUI;

public class Scene
{
	public GUIObject[] Objects = Array.Empty<GUIObject>();

	public void LoadSceneAssets(ContentManager content)
	{
		foreach(GUIObject obj in Objects)
		{
			obj.LoadObjectResource(content);
		}
	}

	public void DrawScene(SpriteBatch spriteBatch)
	{
		foreach(GUIObject obj in Objects)
		{
			obj.DrawObject(spriteBatch);
		}
	}
}