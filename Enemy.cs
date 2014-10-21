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

    public enum EnemyType
    {
        Follower, Wanderer
    }

    public class Enemy : ColoredGameObject
    {
        private Matrix World = Matrix.Identity;
        private Matrix WorldInverseTranspose;
        public float Speed;
        public EnemyType enemyType;

        public Vector3 pos { get; set; }
        public Vector3 velocity { get; set; }
        public double wanderAngle { get; set; }

        public Random r = new Random();

        public void setPos(float x, float y, float z)
        {
            this.pos = new Vector3(x, y, z);
        }

        public Enemy(LabGame game, Vector3 pos,EnemyType enemyType, float speed)
        {
            this.game = game;
            type = GameObjectType.Enemy;
            this.pos = pos;
            this.enemyType = enemyType;

            this.wanderAngle = 0;
            //this.velocity = new Vector3(this.r.NextFloat(-1, 0), 0, this.r.NextFloat(-1, 0));
            this.velocity = new Vector3(0, 0, 0);

            
            setAttribute(enemyType, speed);

            float size = 3;
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

            Color enemyColor = Color.Crimson;
            Color bottomColor = Color.White;
            vertices = Buffer.Vertex.New(
                game.GraphicsDevice,
                new[]
                    {
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, enemyColor), // Front
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, enemyColor),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, enemyColor),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, enemyColor),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, enemyColor), // BACK
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, enemyColor),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, enemyColor),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, enemyColor),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, enemyColor), // Top
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, enemyColor),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, enemyColor),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, bottomColor), // Bottom
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, bottomColor),
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, bottomColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, bottomColor),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, bottomColor),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, bottomColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, enemyColor), // Left
                    new VertexPositionNormalColor(backBottomLeft, backBottomLeftNormal, enemyColor),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, enemyColor),
                    new VertexPositionNormalColor(frontBottomLeft, frontBottomLeftNormal, enemyColor),
                    new VertexPositionNormalColor(backTopLeft, backTopLeftNormal, enemyColor),
                    new VertexPositionNormalColor(frontTopLeft, frontTopLeftNormal, enemyColor),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, enemyColor), // Right
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(backBottomRight, backBottomRightNormal, enemyColor),
                    new VertexPositionNormalColor(frontBottomRight, frontBottomRightNormal, enemyColor),
                    new VertexPositionNormalColor(frontTopRight, frontTopRightNormal, enemyColor),
                    new VertexPositionNormalColor(backTopRight, backTopRightNormal, enemyColor),
                });
            //effect = game.Content.Load<Effect>("Phong");

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            this.game = game;

        }

        public override void Update(GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            
            World = Matrix.Translation(pos);
            WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(World));

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

            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the rotating cube
            this.effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }

        private void setAttribute(EnemyType enemyType, float speed)
        {
            if (enemyType == EnemyType.Follower)
            {
                this.Speed = speed;

            }
            else if (enemyType == EnemyType.Wanderer)
            {
                this.Speed = speed;
            }

        }

        public Boolean isBlock(Vector3 newPos)
        {
            if (newPos.X < 0 || newPos.X > game.landscape.getWidth() || newPos.Z < 0 || newPos.Z > game.landscape.getWidth())
            {
                return false;
            }
            return true;
        }

    }
}