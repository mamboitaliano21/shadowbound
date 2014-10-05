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

    
    // Enemy Controller class.
    class EnemyController : GameObject
    {


        // Constructor.
        public EnemyController(LabGame game)
        {
            this.game = game;

        }




        // Frame update method.
        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime) { }

    }


        // TASK 2.
        // Move all the enemies, changing directions and stepping down when the edge of the screen is reached.

}