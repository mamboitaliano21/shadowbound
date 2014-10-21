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


        private static float CircleDist = 10.0f;
        private static float CircleRadius = 5.0f;
        private static float angleChange = 20.0f;
        private Random r = new Random();
        private float time = 0;
        // Constructor.
        public EnemyController(LabGame game)
        {
            this.type = GameObjectType.Controller;
            this.game = game;

        }


        // Frame update method.
        public override void Update(GameTime gameTime)
        {

            time = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            Random r = new Random();
            float len = displacement.Length();
            displacement.X = (float)(Math.Cos(wanderAngle) * len) * r.NextFloat(-1,1);
            displacement.Z = (float)(Math.Cos(wanderAngle) * len) * r.NextFloat(-1, 1);
            return displacement;
        }

        private Vector3 wanderMove(Enemy e)
        {
            Vector3 circle = circle3D(e.velocity);

            Vector3 displacement = new Vector3(e.r.NextFloat(-1, 1), 0, e.r.NextFloat(-1, 1));
            displacement *= CircleDist;
            displacement = setAngle(displacement, e.wanderAngle);

            e.wanderAngle += ((r.NextFloat(-1,1) * angleChange) - (angleChange * 0.5f));

            Vector3 wanderForce = circle + displacement;

            return wanderForce;
        }

        private Vector3 pursuit(Enemy e, Player p)
        {
            Vector3 distance = (p.pos - e.pos);
            distance.Y = 0;
            float t = distance.Length() / p.MS;
            Vector3 nextPosition = p.pos + p.velocity * t;
            nextPosition.Y = e.pos.Y;
            return seekMove(e, nextPosition);
        }

        private Vector3 seekMove(Enemy e, Vector3 targetPos)
        {
            Vector3 move = targetPos - e.pos;
            move.Normalize();
            move *= e.Speed * time;

            move = move - e.velocity;
            return move;
        }

        // Update the enemy movement
        private void updateMovement(Enemy e)
        {
            Vector3 steering;
            if (e.enemyType == EnemyType.Follower)
            {
                steering = pursuit(e, game.player);
                e.pos += steering;
            }

            else if (e.enemyType == EnemyType.Wanderer)
            {
                steering = wanderMove(e);
                steering.Normalize();
                steering *= 5.0f;
                e.velocity += steering;
                e.velocity.Normalize();
                e.velocity *= e.Speed * time;
                e.pos += e.velocity;
            }

            Vector3 currPos = e.pos;
            if (currPos.X <= 0)
            {
                currPos.X = 0;
            }

            if (currPos.Y <= 0)
            {
                currPos.Y = 0;
            }

            if (currPos.X >= game.landscape.getWidth())
            {
                currPos.X = game.landscape.getWidth();
            }

            if (currPos.Y >= game.landscape.getWidth())
            {
                currPos.Y = game.landscape.getWidth();
            }
            e.pos = currPos;
        }

        

        public override void Draw(GameTime gameTime, Effect effect) { }

    }
}