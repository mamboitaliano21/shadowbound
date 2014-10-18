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
        private static float angleChange = (float)(Math.PI / 180)*90.0f;
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
               // updateMovement(game.enemies[i]);
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
            displacement.X = (float)Math.Cos(wanderAngle) * len;
            displacement.Z = (float)Math.Cos(wanderAngle) * len;
            return displacement;
        }

        private Vector3 wanderMove(Enemy e)
        {
            Vector3 circle = circle3D(e.velocity);
            Random r = new Random();
            e.displacement *= CircleDist;
            
            e.displacement = setAngle(e.displacement, e.wanderAngle);

            double circleMag = Math.Sqrt(Math.Pow(circle.X, 2) + Math.Pow(circle.Y, 2) + Math.Pow(circle.Z, 2));

            double displacementMag = Math.Sqrt(Math.Pow(e.displacement.X, 2) + Math.Pow(e.displacement.Y, 2) + Math.Pow(e.displacement.Z, 2));

            e.wanderAngle = Math.Acos(Vector3.Dot(circle, e.displacement) / (circleMag * displacementMag));


            e.wanderAngle += (Math.PI / 180) * ((r.NextDouble() * angleChange) - (angleChange * 0.5));

            Vector3 wanderForce;
            wanderForce = circle + e.displacement;

            return wanderForce;
        }

        // Update the enemy movement
        //private void updateMovement(Enemy e)
        //{
        //    if (e.enemyType == EnemyType.Follower)
        //    {
                
        //    }

        //    else if (e.enemyType == EnemyType.Wanderer)
        //    {
        //        e.velocity = wanderMove(e);
        //        e.pos += e.velocity;
        //    }
        //}

        public override void Draw(GameTime gameTime, Effect effect) { }

    }


        // TASK 2.
        // Move all the enemies, changing directions and stepping down when the edge of the screen is reached.

}