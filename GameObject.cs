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
        None, Player, Enemy, Controller, Portal
    }

    // Super class for all game objects.
    abstract public class GameObject
    {
        
        public VertexInputLayout inputLayout;
        public LabGame game;
        public GameObjectType type = GameObjectType.None;
        public Effect effect;
        // two effects , landscape is still using basic effect for now

        

        public abstract void Update(GameTime gametime);

        public abstract void Draw(GameTime gametime,Effect effect);


    }
}
