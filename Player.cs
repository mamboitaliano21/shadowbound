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
    class Player : GameObject
    {
        private float speed = 0.006f;
        private float projectileSpeed = 20;

        public Player(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("player", CreatePlayerModel);
            pos = new SharpDX.Vector3(0, game.boundaryBottom + 0.5f, 0);
            GetParamsFromModel();
        }

        public MyModel CreatePlayerModel()
        {
            return game.assets.CreateTexturedCube("player.png", 0.7f);
        }

        // Method to create projectile texture to give to newly created projectiles.
        private MyModel CreatePlayerProjectileModel()
        {
            return game.assets.CreateTexturedCube("player projectile.png", new Vector3(0.3f, 0.2f, 0.25f));
        }

        // Shoot a projectile.
        private void fire()
        {
            game.Add(new Projectile(game,
                game.assets.GetModel("player projectile", CreatePlayerProjectileModel),
                pos,
                new Vector3(0, projectileSpeed, 0),
                GameObjectType.Enemy
            ));
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            if (game.keyboardState.IsKeyDown(Keys.Space)) { fire(); }

            // Determine velocity based on keys being pressed.
            if (game.keyboardState.IsKeyDown(Keys.Left)) { pos.X -= speed * gameTime.ElapsedGameTime.Milliseconds; }
            if (game.keyboardState.IsKeyDown(Keys.Right)) { pos.X += speed * gameTime.ElapsedGameTime.Milliseconds; }

            // Keep within the boundaries.
            if (pos.X < game.boundaryLeft) { pos.X = game.boundaryLeft; }
            if (pos.X > game.boundaryRight) { pos.X = game.boundaryRight; }

            basicEffect.World = Matrix.Translation(pos);
        }

        // React to getting hit by an enemy bullet.
        public void Hit()
        {
            game.Exit();
        }
    }
}