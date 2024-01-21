using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ArcherEngine.Core;

namespace ArcherEngine.Player;

class ArcherGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

	private RenderTarget2D _renderTarget;

    public ArcherGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _renderTarget = new RenderTarget2D(GraphicsDevice, Constants.InternalWidth, Constants.InternalHeight);

		// TODO: Replace this with loading an actual config file.
		ChangeResolution(2);
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
		GraphicsDevice.Clear(Color.CornflowerBlue);
		_spriteBatch.Begin(samplerState: SamplerState.PointClamp);

		// TODO: Sprite batching lol

		_spriteBatch.End();
		GraphicsDevice.SetRenderTarget(null);

		// Render the render target here
		_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
		_spriteBatch.Draw(_renderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
		_spriteBatch.End();

        base.Draw(gameTime);
    }

	public void ChangeResolution(int Multiplier)
	{
		_graphics.PreferredBackBufferWidth = Constants.InternalWidth * Multiplier;
		_graphics.PreferredBackBufferHeight = Constants.InternalHeight * Multiplier;
		_graphics.ApplyChanges();

		System.Console.WriteLine("New resolution: {0} x {1}", _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
	}
}
