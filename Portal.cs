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
    public class Portal : ColoredGameObject
    {
        public Vector3 pos;
        public LabGame game;
        public GameObjectType type;
        public BasicEffect basicEffect;
        Random r = new Random();

        public Portal(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Portal;
            pos = new Vector3(r.Next(0, 200), 0, r.Next(0, 200));

        }
        public void Update(GameTime gametime)
        {
            if (Vector3.Distance(game.player.pos, this.pos) < 1)
            {
                Console.WriteLine("Game Over");
            }
        }

        public void Draw(GameTime gametime)
        {

        }

    }
}
