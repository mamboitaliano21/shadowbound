using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Lab
{
    // Enemy class
    // Basically just move randomly, see EnemyController for enemy movement.
    using SharpDX.Toolkit.Graphics;
    class Enemy : ColoredGameObject
    {
        private Matrix World;
        private Matrix WorldInverseTranspose;

        public Vector3 pos;
        public Enemy(LabGame game, Vector3 pos)
        {
            this.game = game;
            type = GameObjectType.Enemy;
            this.pos = pos;

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
            effect = game.Content.Load<Effect>("Phong");

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            this.game = game;

        }

        public override void Update(GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            World = Matrix.Translation(pos);
            WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(World));

        }

        public override void Draw(GameTime gameTime)
        {
            // Setup the effect parameters
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["Projection"].SetValue(game.camera.Projection);
            effect.Parameters["View"].SetValue(game.camera.View);
            effect.Parameters["cameraPos"].SetValue(game.camera.cameraPos);
            effect.Parameters["worldInvTrp"].SetValue(WorldInverseTranspose);

            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the rotating cube
   
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }

    }
}