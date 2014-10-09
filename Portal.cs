using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Lab
{
    // Portal class
    // Portal 
    using SharpDX.Toolkit.Graphics;
    class Portal : GameObject
    {

        public Vector3 pos;
        public Portal(LabGame game, Vector3 pos)
        {
            this.game = game;
            type = GameObjectType.Enemy;
            this.pos = pos;

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {

        }

    }
}