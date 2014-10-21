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
        private Matrix World = Matrix.Identity;
        private Matrix WorldInverseTranspose;
        private Vector4 cubeCol;

        private Vector4[] colorArray = {Color.Red.ToVector4(), Color.Blue.ToVector4(), Color.Violet.ToVector4(), Color.Green.ToVector4(), Color.Yellow.ToVector4(), Color.Purple.ToVector4(), Color.Chocolate.ToColor4(), Color.Gold.ToVector4()};
        private SoundEffect shotEffect = new SoundEffect(@"Content\eat.wav", false);

        public Portal(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Portal;
            this.pos = new Vector3(r.Next(0, (int)game.landscape.getWidth()), 10, r.Next(0, (int)game.landscape.getWidth()));
            this.cubeCol = randomColor();

            float size = 2;
            Vector3 frontBottomLeft = new Vector3(-size, -size, -size);
            Vector3 frontTopLeft = new Vector3(-size, size, -size);
            Vector3 frontTopRight = new Vector3(size, size, -size);
            Vector3 frontBottomRight = new Vector3(size, -size, -size);
            Vector3 backBottomLeft = new Vector3(-size, -size, size);
            Vector3 backBottomRight = new Vector3(size, -size, size);
            Vector3 backTopLeft = new Vector3(-size, size, size);
            Vector3 backTopRight = new Vector3(size, size, size);

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

            Color portalColor = Color.White;
            vertices = Buffer.Vertex.New(
                game.GraphicsDevice,
                new[]
                    {
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, portalColor), // Front
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, portalColor),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, portalColor),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, portalColor),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, portalColor), // BACK
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, portalColor),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, portalColor),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, portalColor),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, portalColor),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, portalColor),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, portalColor), // Top
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, portalColor),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, portalColor),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, portalColor),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, portalColor),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, portalColor), // Bottom
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, portalColor),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, portalColor),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, portalColor), // Left
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, portalColor),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, portalColor),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, portalColor),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, portalColor), // Right
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, portalColor),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, portalColor),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, portalColor),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, portalColor),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, portalColor),
                });

            //basicEffect = new BasicEffect(game.GraphicsDevice)
            //{
            //    VertexColorEnabled = true,
            //    View = Matrix.LookAtLH(game.camera.cameraPos, game.camera.cameraTarget, Vector3.UnitY),
            //    Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f),
            //    World = Matrix.Identity

            //};

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);

        }



        public override void Update(GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            World = Matrix.Translation(pos);
            WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(World));
            if (Vector3.Distance(game.player.pos, this.pos) < 5)
            {
                portalHit();
            }

        }

        public override void Draw(GameTime gameTime, Effect effect)
        {
            this.effect = effect;
            // Setup the effect parameters
            this.effect.Parameters["World"].SetValue(World);
            //effect.Parameters["Projection"].SetValue(game.camera.Projection);
            //effect.Parameters["View"].SetValue(game.camera.View);
            //effect.Parameters["cameraPos"].SetValue(game.camera.cameraPos);
            this.effect.Parameters["worldInvTrp"].SetValue(WorldInverseTranspose);
            this.effect.Parameters["lightPntPos"].SetValue(this.pos);
            this.effect.Parameters["cubeCol"].SetValue(this.cubeCol);

            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the rotating cube
            this.effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }



        private void portalHit()
        {
            this.pos = new Vector3(r.Next(0, (int)game.landscape.getWidth()), 10, r.Next(0, (int)game.landscape.getWidth()));
            this.game.score += 100;
            shotEffect.Play();
            this.cubeCol = randomColor();
        }

        private Vector4 randomColor()
        {
            return colorArray[game.random.Next(colorArray.Length)];
        }
    }
}
