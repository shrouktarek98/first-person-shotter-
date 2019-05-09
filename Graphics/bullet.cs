using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmNet;

namespace Graphics
{
    class bullet
    {
        Camera c = new Camera();
        vec3 pos;
        vec3 dir;
        public bullet(vec3 cp,vec3 ld)
        {
            pos = cp;
            dir = ld;
        }
        public void draw(Model3D m, int matID)
        {
            m.Draw(matID);
        }
        public void update()
        {
            pos += dir;
        }

    }
}
