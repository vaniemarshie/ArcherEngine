using ArcherEngine.Core.GUI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcherEngine.Player;

class GUIManager
{
	private Scene _scene;
	private readonly ContentManager _content;

	public GUIManager(string startingScene, ContentManager content) {
		_content = content;
		LoadScene(startingScene);
	}

	public void LoadScene(string sceneName)
	{
		_scene = _content.Load<Scene>("Scenes/" + sceneName);
		_scene.LoadSceneAssets(_content);
	}

	public void DrawScene(SpriteBatch spriteBatch) => _scene.DrawScene(spriteBatch);
}