using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Lab
{
    using SharpDX.Toolkit.Graphics;
    abstract public class ColoredGameObject : GameObject
    {
        public Buffer<VertexPositionNormalColor> vertices;
    }
}
