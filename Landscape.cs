using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Lab
{
    using SharpDX.Toolkit.Graphics;
    public class Landscape : TextureGameObject
    {
        private Matrix World;
        private Matrix WorldInverseTranspose;

        private static float LANDSCAPEHEIGHT = 0.0f;
        public float LANDSCAPEWIDTH = 500.0f;

        //private Vector3 initialPos = new Vector3(0, 100, -20);
        //private Vector3 initialCamera = new Vector3(0, 100, 0);

        public float getHeight()
        {
            return LANDSCAPEHEIGHT;
        }

        public float getWidth()
        {
            return LANDSCAPEWIDTH;
        }

        public Landscape(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Landscape;
   
            List<VertexPositionNormalTexture> landscapeVertices = new List<VertexPositionNormalTexture>();
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(0.0f, LANDSCAPEHEIGHT, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f,0.0f)));
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(0.0f, LANDSCAPEHEIGHT, LANDSCAPEWIDTH), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)));
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(LANDSCAPEWIDTH, LANDSCAPEHEIGHT, LANDSCAPEWIDTH), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)));

            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(0.0f, LANDSCAPEHEIGHT, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)));
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(LANDSCAPEWIDTH, LANDSCAPEHEIGHT, LANDSCAPEWIDTH), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)));
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(LANDSCAPEWIDTH, LANDSCAPEHEIGHT, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f)));
           
            vertices = Buffer.Vertex.New(
                game.GraphicsDevice,
               landscapeVertices.ToArray());

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            
        }

        public override void Update(GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            World = Matrix.Identity;
            WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(World));

        }

        public override void Draw(GameTime gameTime, Effect effect)
        {
            this.effect = effect;

            // Setup the effect parameters
            this.effect.Parameters["World"].SetValue(World);
            this.effect.Parameters["worldInvTrp"].SetValue(WorldInverseTranspose);

            // Setup the vertices
            this.game.GraphicsDevice.SetVertexBuffer(vertices);
            this.game.GraphicsDevice.SetVertexInputLayout(inputLayout);
    
            foreach(EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
            }
        }
    }
}
