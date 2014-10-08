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
        public LabGame game;
        public Vector3 cameraPos;
        public Vector3 cameraTarget;


        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(LabGame game) {

            this.cameraPos = game.player.pos;
            this.cameraTarget = cameraPos + new Vector3(0, 0, 10);
            View = Matrix.LookAtLH(cameraPos, cameraTarget, Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            this.game = game;
        }

        // If the screen is resized, the projection matrix will change
        public void Update()
        {
            this.cameraPos = game.player.pos;
            this.cameraTarget = cameraPos + new Vector3(0, 0, 10);

            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            View = Matrix.LookAtLH(cameraPos, cameraTarget , Vector3.UnitY);
            
        }
    }
}
