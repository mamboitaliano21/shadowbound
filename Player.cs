﻿using System;
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
    using System.Diagnostics;
    // Player class.
    public class Player : GameObject
    {
        private float sqrt_two = (float) Math.Sqrt(2);
        private const int MS = 90;
        private const float SENSITIVITY = 0.03f;
        private Boolean devMode = false;

        // player position and eye target
        public Vector3 pos;
        public Vector3 target;

        // direction x, y, z axis of the player
        private Vector3 XAxis;
        private Vector3 YAxis; // TODO kyknya ga penting
        private Vector3 ZAxis;

        public Player(LabGame game)
        {
            this.pos = new Vector3(16,10,0);
            this.target = new Vector3(16, 10, 10);
            this.XAxis = Vector3.UnitX;
            this.YAxis = Vector3.UnitY;
            this.ZAxis = Vector3.UnitZ;
            this.game = game;
            type = GameObjectType.Player;
            
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            if (devMode) {

                //Enable debugging of position
                Debug.WriteLine(pos);
                var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                //TODO bad coding removing fire
                if (game.keyboardState.IsKeyDown(Keys.Space)) {  } 

                // Determine velocity based on keys being pressed.
                float dz = 0, dx = 0;
                if (game.keyboardState.IsKeyDown(Keys.W)) {
                    dz += 1;
                }
                if (game.keyboardState.IsKeyDown(Keys.S))
                {
                    dz -= 1;
                }
                if (game.keyboardState.IsKeyDown(Keys.D))
                {
                    dx += 1;
                }
                if (game.keyboardState.IsKeyDown(Keys.A)) {
                    dx -= 1;
                }

                if ((dz != 0) && (dx != 0))
                {
                    dz /= sqrt_two;
                    dx /= sqrt_two;
                }

                // Update pos and target based on velocity
                Vector3 change = (dx * MS * time * XAxis) + (dz * MS * time * ZAxis);
                pos += change;
                target += change;

                // Change direction player is facing
                int dRotationY = 0, dRotationX = 0;
                if (game.keyboardState.IsKeyDown(Keys.Left)) {
                    dRotationY--;
                }
                if (game.keyboardState.IsKeyDown(Keys.Right))
                {
                    dRotationY++;
                }
                if (game.keyboardState.IsKeyDown(Keys.Up))
                {
                    dRotationX--;
                }
                if (game.keyboardState.IsKeyDown(Keys.Down))
                {
                    dRotationX++;
                }

                Matrix rotationMatrix = Matrix.RotationAxis(YAxis, dRotationY*SENSITIVITY) * Matrix.RotationAxis(XAxis, dRotationX*SENSITIVITY);
                target = Vector3.TransformCoordinate(target - pos, rotationMatrix) + pos; // rotation of target about pos
                XAxis = Vector3.TransformCoordinate(XAxis, rotationMatrix);
                //YAxis = Vector3.TransformCoordinate(YAxis, rotationMatrix); // TODO kyknya ga perlu
                ZAxis = Vector3.TransformCoordinate(ZAxis, rotationMatrix);

                // TODO
                // Keep within the boundaries.
                if (pos.X < game.boundaryLeft) { }
                if (pos.X > game.boundaryRight) {  }

                //basicEffect.World = Matrix.Translation(pos);
            } else {
                //Enable debugging of position
                //Debug.WriteLine(pos);
                var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                //TODO bad coding removing fire
                if (game.keyboardState.IsKeyDown(Keys.Space)) {  } 

                // Determine velocity based on keys being pressed.
                float dz = 0, dx = 0;
                if (game.keyboardState.IsKeyDown(Keys.W)) {
                    dz += 1;
                }
                if (game.keyboardState.IsKeyDown(Keys.S))
                {
                    dz -= 1;
                }
                if (game.keyboardState.IsKeyDown(Keys.D))
                {
                    dx += 1;
                }
                if (game.keyboardState.IsKeyDown(Keys.A)) {
                    dx -= 1;
                }
                
                if ((dz != 0) && (dx != 0))
                {
                    dz /= sqrt_two;
                    dx /= sqrt_two;
                }

                // Update pos and target based on velocity
                Vector3 changeX = dx * MS * time * XAxis;
                Vector3 changeZ = dz * MS * time * ZAxis;
                if ((changeX.X >= 0) && (changeX.X <= this.game.landscape.getHeight()))
                {
                    pos += changeX;
                    target += changeX;
                }
                if ((changeZ.Z >= 0) && (changeX.X <= this.game.landscape.getWidth()))
                {
                    pos += changeZ;
                    target += changeZ;
                }

                // Change direction player is facing
                int dRotationY = 0, dRotationX = 0;
                if (game.keyboardState.IsKeyDown(Keys.Left)) {
                    dRotationY--;
                }
                if (game.keyboardState.IsKeyDown(Keys.Right))
                {
                    dRotationY++;
                }
                if (game.keyboardState.IsKeyDown(Keys.Up))
                {
                    dRotationX--;
                }
                if (game.keyboardState.IsKeyDown(Keys.Down))
                {
                    dRotationX++;
                }
                Matrix rotationMatrixX = Matrix.RotationAxis(XAxis, dRotationX * SENSITIVITY);
                Matrix rotationMatrixY = Matrix.RotationAxis(YAxis, dRotationY * SENSITIVITY);
                Vector3 eyeDirection = Vector3.TransformCoordinate(target - pos, rotationMatrixX * rotationMatrixY);
                float theta = (float) Math.Acos(Vector3.Dot(eyeDirection, Vector3.UnitZ) / (eyeDirection.Length() * Vector3.UnitZ.Length())) * (float) (180 / Math.PI);
                if (!(theta >= 90))
                {
                    target = eyeDirection + pos; // rotation of target about pos
                    XAxis = Vector3.TransformCoordinate(XAxis, rotationMatrixX * rotationMatrixY);
                    //YAxis = Vector3.TransformCoordinate(YAxis, rotationMatrix); // TODO kyknya ga perlu
                    ZAxis = Vector3.TransformCoordinate(ZAxis, rotationMatrixY);
                }

                // TODO
                // Keep within the boundaries.
                if (pos.X < game.boundaryLeft) { }
                if (pos.X > game.boundaryRight) {  }

                //basicEffect.World = Matrix.Translation(pos);
            }
        }

        public override void Draw(GameTime gameTime,Effect effect)
        {

        }

        // React to getting hit by an enemy bullet.
  
    }
}