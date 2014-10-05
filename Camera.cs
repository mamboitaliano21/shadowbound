using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
namespace Lab
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public Game game;
        public Vector3 cameraPos;


        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(Game game) {

            this.cameraPos = game.player.pos;
            View = Matrix.LookAtLH(cameraPos, new Vector3(0, 0, 0), Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            this.game = game;
        }

        // If the screen is resized, the projection matrix will change
        public void Update()
        {
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            
        }
    }
}
