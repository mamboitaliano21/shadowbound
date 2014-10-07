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
    using SharpDX.Toolkit.Input;
    // Player class.
    public class Player : GameObject
    {
        public Vector3 pos;

        public Player(LabGame game)
        {
            this.pos = new Vector3(0,0,-10);
            this.game = game;
            type = GameObjectType.Player;
            

        }





        // Frame update.
        public override void Update(GameTime gameTime)
        {
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //TODO bad coding removing fire
            if (game.keyboardState.IsKeyDown(Keys.Space)) {  } 

            // Determine velocity based on keys being pressed.
            if (game.keyboardState.IsKeyDown(Keys.W)) { pos += 1 * time; }
            if (game.keyboardState.IsKeyDown(Keys.Left)) { }
            if (game.keyboardState.IsKeyDown(Keys.Right)) {  }

            // Keep within the boundaries.
            if (pos.X < game.boundaryLeft) { }
            if (pos.X > game.boundaryRight) {  }

            //basicEffect.World = Matrix.Translation(pos);
        }

        public override void Draw(GameTime gameTime)
        {

        }

        // React to getting hit by an enemy bullet.
  
    }
}