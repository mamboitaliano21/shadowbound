using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX;
using SharpDX.Toolkit;

namespace Lab
{
    using SharpDX.Toolkit.Graphics;

    
    // Enemy Controller class.
    // A singleton 
    public class EnemyController : GameObject
    {


        private static float CircleDist = 1.0f;
        private static float CircleRadius = 1.0f;
        private static float angleChange = 10.0f;
        private Random r = new Random();
        // Constructor.
        public EnemyController(LabGame game)
        {
            this.type = GameObjectType.Controller;
            this.game = game;

        }


        // Frame update method.
        public override void Update(GameTime gameTime)
        {
        
            for (int i = 0; i < game.enemies.Count; i++)
            {
                //game.enemies[i].pos += new Vector3(1, 0, 0);
               updateMovement(game.enemies[i]);
            }
        
        }

        private Vector3 circle3D(Vector3 velocity)
        {
            Vector3 result = velocity;
            result.Normalize();
            result *= CircleDist;
            return result;
        }

        private Vector3 setAngle(Vector3 displacement, double wanderAngle)
        {
            
            float len = displacement.Length();
            displacement.X = (float)(Math.Cos(wanderAngle) * len);
            displacement.Z = (float)(Math.Cos(wanderAngle) * len);
            return displacement;
        }

        private Vector3 wanderMove(Enemy e)
        {
            Vector3 circle = circle3D(e.velocity);

            Vector3 displacement = new Vector3(0, 0, -1);
            displacement *= CircleDist;

            displacement = setAngle(displacement, e.wanderAngle);

            e.wanderAngle += (r.NextDouble() * angleChange) - (angleChange * 0.5);

            Vector3 wanderForce = circle + displacement;

            return wanderForce;
        }

        // Update the enemy movement
        private void updateMovement(Enemy e)
        {
            if (e.enemyType == EnemyType.Follower)
            {
                
            }

            else if (e.enemyType == EnemyType.Wanderer)
            {
                //e.pos += e.pos;
                Vector3 steering = wanderMove(e);
                steering.Normalize();
                steering *= 2.0f;
                e.velocity += steering;
                e.velocity.Normalize();
                e.velocity *= e.Speed;
                e.pos += e.velocity;
            }
        }

        public override void Draw(GameTime gameTime, Effect effect) { }

    }
}