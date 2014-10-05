using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Lab
{
    using SharpDX.Toolkit.Graphics;
    class Landscape : ColoredGameObject
    {
        private int mapWidth = 513;
        private int mapHeight = 513;
        private int mountainHeight = 200;
        private int maxDepth = -100;

        private Vector3 initialPos = new Vector3(0, 100, -20);
        private Vector3 initialCamera = new Vector3(0, 100, 0);

        public BoundingSphere boundingSphere;
        public BoundingBox boundingBox;

        public Landscape(LabGame game)
        {
            this.boundingSphere = new BoundingSphere(initialPos, 25);
            this.boundingBox = new BoundingBox(new Vector3(-mapWidth - 1000000, -300, -mapHeight - 1000000), new Vector3(mapWidth + 1000000, 0, mapHeight + 1000000));

            
            //H-value,mapwidth,mapheight,iteration,maximum depth,maximum height
            TerrainGenerator tg = new TerrainGenerator(0.5, mapWidth, mapHeight, 100, maxDepth, mountainHeight);
            float[,] arr = tg.generateTerrain();
            

            List<VertexPositionNormalColor> list = new List<VertexPositionNormalColor>();
            for (int j = 0; j<mapHeight; j++){
                if (j + 1 < mapHeight)
                {
                    for (int i = 0; i < mapWidth; i++)
                    {
                        if (i + 1 < mapWidth)
                        {
                            Vector3 p1 = new Vector3(i, (float)arr[i, j],j );
                            Vector3 p2 = new Vector3(i + 1, (float)arr[i + 1, j],j );
                            Vector3 p3 = new Vector3(i + 1, (float)arr[i + 1, j + 1],j+1 );
                            Vector3 p4 = new Vector3(i, (float)arr[i, j + 1],j + 1);
                            // calculate normal
                            //calculate color
                            Vector3 Normal = Vector3.Cross(p2 - p1, p4 - p1);
                            VertexPositionNormalColor vertex1 = new VertexPositionNormalColor(p1, Normal, decideColor(arr[i,j]) );
                            VertexPositionNormalColor vertex2 = new VertexPositionNormalColor(p4, Normal, decideColor(arr[i, j+1]));
                            VertexPositionNormalColor vertex3 = new VertexPositionNormalColor(p3, Normal, decideColor(arr[i+1, j+1]));
                            VertexPositionNormalColor vertex4 = new VertexPositionNormalColor(p1, Normal, decideColor(arr[i, j]));
                            VertexPositionNormalColor vertex5 = new VertexPositionNormalColor(p3, Normal, decideColor(arr[i+1, j+1]));
                            VertexPositionNormalColor vertex6 = new VertexPositionNormalColor(p2, Normal, decideColor(arr[i+1, j]));
                            list.Add(vertex1);
                            list.Add(vertex2);
                            list.Add(vertex3);
                            list.Add(vertex4);
                            list.Add(vertex5);
                            list.Add(vertex6);
                        }
                    }
                }
            }
            

            //add the water
            int waterHeight = (mountainHeight + maxDepth)/3;
            Vector3 waterP1 = new Vector3(0, waterHeight , 0);
            Vector3 waterP2 = new Vector3(mapWidth,waterHeight, 0);
            Vector3 waterP3 = new Vector3(mapWidth, waterHeight, mapHeight);
            Vector3 waterP4 = new Vector3(0, waterHeight, mapHeight);

            Vector3 waterNormal = Vector3.Cross(waterP2 - waterP1, waterP4 - waterP1);

            VertexPositionNormalColor water1 = new VertexPositionNormalColor(waterP1, waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor water2 = new VertexPositionNormalColor(waterP4, waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor water3 = new VertexPositionNormalColor(waterP3, waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor water4 = new VertexPositionNormalColor(waterP1, waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor water5 = new VertexPositionNormalColor(waterP3, waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor water6 = new VertexPositionNormalColor(waterP2, waterNormal, new Color(10, 105, 148, 125));

            VertexPositionNormalColor reverseWater1 = new VertexPositionNormalColor(waterP1, -waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor reverseWater2 = new VertexPositionNormalColor(waterP3, -waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor reverseWater3 = new VertexPositionNormalColor(waterP4, -waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor reverseWater4 = new VertexPositionNormalColor(waterP1, -waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor reverseWater5 = new VertexPositionNormalColor(waterP2, -waterNormal, new Color(10, 105, 148, 125));
            VertexPositionNormalColor reverseWater6 = new VertexPositionNormalColor(waterP3, -waterNormal, new Color(10, 105, 148, 125));

            list.Add(water1);
            list.Add(water2);
            list.Add(water3);
            list.Add(water4);
            list.Add(water5);
            list.Add(water6);

            list.Add(reverseWater1);
            list.Add(reverseWater2);
            list.Add(reverseWater3);
            list.Add(reverseWater4);
            list.Add(reverseWater5);
            list.Add(reverseWater6);


            VertexPositionNormalColor[] landscapeVertices = list.ToArray();
            vertices = Buffer.Vertex.New(
                game.GraphicsDevice,
               landscapeVertices);

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                VertexColorEnabled = true,
                View = Matrix.LookAtLH(initialPos, initialCamera, Vector3.UnitY),
                Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f),
                World = Matrix.Identity

            };
           

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            this.game = game;
        }

        public override void Update(GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
    
            basicEffect.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f);

            basicEffect.LightingEnabled = true;
            basicEffect.PreferPerPixelLighting = true;
            basicEffect.VertexColorEnabled = true;

            basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.35f, 0.35f, 0.35f);
            basicEffect.DirectionalLight0.Direction = new Vector3(0, (float) Math.Sin(time), 0);
            basicEffect.DirectionalLight0.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
        }

        public override void Draw(GameTime gameTime)
        {
            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);
            game.GraphicsDevice.SetBlendState(game.GraphicsDevice.BlendStates.AlphaBlend);
            // Apply the basic effect technique and draw the rotating cube
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
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
