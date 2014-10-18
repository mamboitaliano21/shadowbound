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
    // A singleton 
    public class EnemyController : GameObject
    {
        private static EnemyController instance = null;
        // Constructor.
        private EnemyController(LabGame game)
        {
            this.type = GameObjectType.Controller;
            this.game = game;

        }

        public static EnemyController getInstance(LabGame game)
        {
            if (instance == null)
            {
                instance = new EnemyController(game);
            }
            return instance;
        }




        // Frame update method.
        public override void Update(GameTime gameTime)
        {
        
            for (int i = 0; i < game.enemies.Count; i++)
            {
                updateMovement(game.enemies[i]);
            }
        
        }
        // Update the enemy movement
        private void updateMovement(Enemy e)
        {
            if (e.enemyType == EnemyType.Follower)
            {
                e.pos += new Vector3(e.Speed, 0, 0);
            }

            else if (e.enemyType == EnemyType.Wanderer)
            {
                e.pos += new Vector3(-e.Speed, 0, 0);
            }
        }

        public override void Draw(GameTime gameTime, Effect effect) { }

    }


        // TASK 2.
        // Move all the enemies, changing directions and stepping down when the edge of the screen is reached.

}