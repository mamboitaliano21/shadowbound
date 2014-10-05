using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Lab
{
    using SharpDX.Toolkit.Graphics;
    public enum GameObjectType
    {
        None, Player, Enemy
    }

    // Super class for all game objects.
    abstract public class GameObject
    {
        public MyModel myModel;
        public LabGame game;
        public GameObjectType type = GameObjectType.None;
        public Vector3 pos;
        public BasicEffect basicEffect;

        public abstract void Update(GameTime gametime);
        public void Draw(GameTime gametime)
        {
            // Some objects such as the Enemy Controller have no model and thus will not be drawn
            if (myModel != null)
            {
                // Setup the vertices
                game.GraphicsDevice.SetVertexBuffer(0, myModel.vertices, myModel.vertexStride);
                game.GraphicsDevice.SetVertexInputLayout(myModel.inputLayout);

                // Apply the basic effect technique and draw the object
                basicEffect.CurrentTechnique.Passes[0].Apply();
                game.GraphicsDevice.Draw(PrimitiveType.TriangleList, myModel.vertices.ElementCount);
            }
        }

        public void GetParamsFromModel()
        {
            if (myModel.modelType == ModelType.Colored) {
                basicEffect = new BasicEffect(game.GraphicsDevice)
                {
                    View = game.camera.View,
                    Projection = game.camera.Projection,
                    World = Matrix.Identity,
                    VertexColorEnabled = true
                };
            }
            else if (myModel.modelType == ModelType.Textured) {
                basicEffect = new BasicEffect(game.GraphicsDevice)
                {
                    View = game.camera.View,
                    Projection = game.camera.Projection,
                    World = Matrix.Identity,
                    Texture = myModel.Texture,
                    TextureEnabled = true,
                    VertexColorEnabled = false
                };
            }
        }
    }
}
