using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StrideGame;

public class TeapotDemo : Game
{
    private Matrix view = Matrix.LookAtRH(new Vector3(0, 0, 5), new Vector3(0, 0, 0), Vector3.UnitY);
    private EffectInstance simpleEffect;
    private GeometricPrimitive teapot;
    private float _time;

    public bool Paused { get; set; }

    protected async override Task LoadContent()
    {
        await base.LoadContent();

        // Prepare effect/shader
        simpleEffect = new EffectInstance(new Effect(GraphicsDevice, SpriteEffect.Bytecode));

        // Load texture
        using (var stream = new FileStream("small_uv.png", FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            simpleEffect.Parameters.Set(TexturingKeys.Texture0, Texture.Load(GraphicsDevice, stream));
        }

        // Initialize teapot
        teapot = GeometricPrimitive.Teapot.New(GraphicsDevice);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        var delta = gameTime.Elapsed.TotalSeconds;
        if (!Paused)
            _time += (float)delta;

        // Clear screen
        GraphicsContext.CommandList.Clear(GraphicsDevice.Presenter.BackBuffer, Color.Black);
        GraphicsContext.CommandList.Clear(GraphicsDevice.Presenter.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer | DepthStencilClearOptions.Stencil);

        // Set render target
        GraphicsContext.CommandList.SetRenderTargetAndViewport(GraphicsDevice.Presenter.DepthStencilBuffer, GraphicsDevice.Presenter.BackBuffer);

        // Compute matrices
        var world = Matrix.Scaling((float)Math.Sin(_time * 1.5f) * 0.2f + 1.0f) * Matrix.RotationX(_time) * Matrix.RotationY(_time * 2.0f) * Matrix.RotationZ(_time * .7f) * Matrix.Translation(0, 0, 0);
        var projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.Presenter.BackBuffer.ViewWidth / GraphicsDevice.Presenter.BackBuffer.ViewHeight, 0.1f, 100.0f);

        // Setup effect/shader
        simpleEffect.Parameters.Set(SpriteBaseKeys.MatrixTransform, Matrix.Multiply(world, Matrix.Multiply(view, projection)));
        simpleEffect.UpdateEffect(GraphicsDevice);

        // Draw
        teapot.Draw(GraphicsContext, simpleEffect);
    }
}
