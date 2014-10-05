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
    // Basically just shoots randomly, see EnemyController for enemy movement.
    using SharpDX.Toolkit.Graphics;
    class Enemy : GameObject
    {
        private float projectileSpeed = 10;

        float fireTimer;
        float fireWaitMin = 2000;
        float fireWaitMax = 20000;

        public Enemy(LabGame game, Vector3 pos)
        {
            this.game = game;
            type = GameObjectType.Enemy;
            myModel = game.assets.GetModel("ship", CreateEnemyModel);
            this.pos = pos;
            setFireTimer();
            GetParamsFromModel();
        }

        private void setFireTimer()
        {
            fireTimer = fireWaitMin + (float)game.random.NextDouble() * (fireWaitMax - fireWaitMin);
        }

        public MyModel CreateEnemyModel()
        {
            return game.assets.CreateTexturedCube("enemy.png", 0.5f);
        }

        private MyModel CreateEnemyProjectileModel()
        {
            return game.assets.CreateTexturedCube("enemy projectile.png", new Vector3(0.2f, 0.2f, 0.4f));
        }

        public override void Update(GameTime gameTime)
        {
            fireTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (fireTimer < 0)
            {
                fire();
                setFireTimer();
            }
            basicEffect.World = Matrix.Translation(pos);
        }

        private void fire()
        {
            int y = 0;
        }

        public void Hit()
        {
            game.Remove(this);
        }

    }
}
