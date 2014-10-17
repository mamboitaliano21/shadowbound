using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Lab
{
    using SharpDX.Toolkit.Graphics;
    class Landscape : TextureGameObject
    {
        private Matrix World;
        private Matrix WorldInverseTranspose;

        private int mapWidth = 513;
        private int mapHeight = 513;
        private int mountainHeight = 200;

        private Vector3 initialPos = new Vector3(0, 100, -20);
        private Vector3 initialCamera = new Vector3(0, 100, 0);

        public Landscape(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Landscape;
            //H-value,mapwidth,mapheight,iteration,maximum depth,maximum height
            //TerrainGenerator tg = new TerrainGenerator(200);
            //float[,] arr = tg.generateTerrain();
            //VertexPositionNormalColor[] landscapeVertices = tg.generateVertex(arr);


            /*List<VertexPositionNormalColor> landscapeVertices = new List<VertexPositionNormalColor>();
            landscapeVertices.Add(new VertexPositionNormalColor(new Vector3(0.0f, 10.0f,0.0f), new Vector3(0.0f, 1.0f,0.0f), Color.Green));
            landscapeVertices.Add(new VertexPositionNormalColor(new Vector3(0.0f, 10.0f, 200.0f), new Vector3(0.0f, 1.0f, 0.0f), Color.Green));
            landscapeVertices.Add(new VertexPositionNormalColor(new Vector3(200.0f, 10.0f, 200.0f), new Vector3(0.0f, 1.0f, 0.0f), Color.Green));
            
            landscapeVertices.Add(new VertexPositionNormalColor(new Vector3(0.0f, 10.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), Color.Green));
            landscapeVertices.Add(new VertexPositionNormalColor(new Vector3(200.0f, 10.0f, 200.0f), new Vector3(0.0f, 1.0f, 0.0f), Color.Green));
            landscapeVertices.Add(new VertexPositionNormalColor(new Vector3(200.0f, 10.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), Color.Green));
            */
            List<VertexPositionNormalTexture> landscapeVertices = new List<VertexPositionNormalTexture>();
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(0.0f, 10.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f,0.0f)));
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(0.0f, 10.0f, 200.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)));
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(200.0f, 10.0f, 200.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)));

            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(0.0f, 10.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)));
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(200.0f, 10.0f, 200.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)));
            landscapeVertices.Add(new VertexPositionNormalTexture(new Vector3(200.0f, 10.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f)));
           
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

            // Apply the basic effect technique and draw the rotating cube
            //effect.CurrentTechnique.Passes[0].Apply();
            
            
            foreach(EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
                /*this.game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices, 0, 1, 
                    VertexPositionNormalTexture.VertexDeclaration);*/
            }
            
            //this.game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }

        private Color decideColor(double height)
        {
            Color textureColor = Color.Green;
            if (height < mountainHeight / 4)
            {
                textureColor = Color.Green;
            }
            else if (height < mountainHeight / 2)
            {
                textureColor = Color.Yellow; 
            }
            else if (height < mountainHeight / 1.5)
            {
                
                textureColor = Color.Gray;
            }
            else 
            {
                textureColor = Color.Snow;
            }

            return textureColor;
        }
    }
}
