using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Lab
{
    using SharpDX.Toolkit.Graphics;
    public class Portal : ColoredGameObject
    {
        public Vector3 pos;
        Random r = new Random();

        public Portal(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Portal;
            //this.pos = new Vector3(r.Next(0, 10), 10, r.Next(0, 10));
            this.pos = new Vector3(0,0,0);

            Vector3 frontBottomLeft = new Vector3(-1.0f, -1.0f, -1.0f);
            Vector3 frontTopLeft = new Vector3(-1.0f, 1.0f, -1.0f);
            Vector3 frontTopRight = new Vector3(1.0f, 1.0f, -1.0f);
            Vector3 frontBottomRight = new Vector3(1.0f, -1.0f, -1.0f);
            Vector3 backBottomLeft = new Vector3(-1.0f, -1.0f, 1.0f);
            Vector3 backBottomRight = new Vector3(1.0f, -1.0f, 1.0f);
            Vector3 backTopLeft = new Vector3(-1.0f, 1.0f, 1.0f);
            Vector3 backTopRight = new Vector3(1.0f, 1.0f, 1.0f);

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 backNormal = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

            Vector3 frontBottomLeftNormal = new Vector3(-0.333f, -0.333f, -0.333f);
            Vector3 frontTopLeftNormal = new Vector3(-0.333f, 0.333f, -0.333f);
            Vector3 frontTopRightNormal = new Vector3(0.333f, 0.333f, -0.333f);
            Vector3 frontBottomRightNormal = new Vector3(0.333f, -0.333f, -0.333f);
            Vector3 backBottomLeftNormal = new Vector3(-0.333f, -0.333f, 0.333f);
            Vector3 backBottomRightNormal = new Vector3(0.333f, -0.333f, 0.333f);
            Vector3 backTopLeftNormal = new Vector3(-0.333f, 0.333f, 0.333f);
            Vector3 backTopRightNormal = new Vector3(0.333f, 0.333f, 0.333f);

            vertices = Buffer.Vertex.New(
                game.GraphicsDevice,
                new[]
                    {
                    new VertexPositionNormalColor(frontBottomLeft, frontNormal, Color.Orange), // Front
                    new VertexPositionNormalColor(frontTopLeft, frontNormal, Color.Orange),
                    new VertexPositionNormalColor(frontTopRight, frontNormal, Color.Orange),
                    new VertexPositionNormalColor(frontBottomLeft, frontNormal, Color.Orange),
                    new VertexPositionNormalColor(frontTopRight, frontNormal, Color.Orange),
                    new VertexPositionNormalColor(frontBottomRight, frontNormal, Color.Orange),
                    new VertexPositionNormalColor(backBottomLeft, backNormal, Color.Orange), // BACK
                    new VertexPositionNormalColor(backTopRight, backNormal, Color.Orange),
                    new VertexPositionNormalColor(backTopLeft, backNormal, Color.Orange),
                    new VertexPositionNormalColor(backBottomLeft, backNormal, Color.Orange),
                    new VertexPositionNormalColor(backBottomRight, backNormal, Color.Orange),
                    new VertexPositionNormalColor(backTopRight, backNormal, Color.Orange),
                    new VertexPositionNormalColor(frontTopLeft, topNormal, Color.OrangeRed), // Top
                    new VertexPositionNormalColor(backTopLeft, topNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(backTopRight, topNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontTopLeft, topNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(backTopRight, topNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontTopRight, topNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontBottomLeft, bottomNormal, Color.OrangeRed), // Bottom
                    new VertexPositionNormalColor(backBottomRight, bottomNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(backBottomLeft, bottomNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontBottomLeft,bottomNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontBottomRight, bottomNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(backBottomRight, bottomNormal, Color.OrangeRed),
                    new VertexPositionNormalColor(frontBottomLeft, leftNormal, Color.DarkOrange), // Left
                    new VertexPositionNormalColor(backBottomLeft, leftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(backTopLeft, leftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontBottomLeft, leftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(backTopLeft, leftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontTopLeft, leftNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontBottomRight, rightNormal, Color.DarkOrange), // Right
                    new VertexPositionNormalColor(backTopRight, rightNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(backBottomRight, rightNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontBottomRight, rightNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(frontTopRight, rightNormal, Color.DarkOrange),
                    new VertexPositionNormalColor(backTopRight, rightNormal, Color.DarkOrange),
                });

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                VertexColorEnabled = true,
                View = Matrix.LookAtLH(game.camera.cameraPos, game.camera.cameraTarget, Vector3.UnitY),
                Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f),
                World = Matrix.Identity

            };
            
            inputLayout = VertexInputLayout.FromBuffer(0, vertices);

        }
        public override void Update(GameTime gametime)
        {
            var time = (float)gametime.TotalGameTime.TotalSeconds;
    
            basicEffect.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f);

            basicEffect.LightingEnabled = true;
            basicEffect.PreferPerPixelLighting = true;
            basicEffect.VertexColorEnabled = true;

            basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.35f, 0.35f, 0.35f);
            basicEffect.DirectionalLight0.Direction = new Vector3(0, (float) Math.Sin(time), 0);
            basicEffect.DirectionalLight0.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
            if (Vector3.Distance(game.player.pos, this.pos) < 1)
            {
                gameEnd();
            }
        }
            
        public override void Draw(GameTime gametime)
        {
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);
            game.GraphicsDevice.SetBlendState(game.GraphicsDevice.BlendStates.AlphaBlend);
            // Apply the basic effect technique and draw the rotating cube
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }

        private void gameEnd()
        {
            game.Exit();
        }
    }
}
