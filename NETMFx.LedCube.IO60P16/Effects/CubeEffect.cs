using NETMFx.LedCube.IO60P16;

namespace NETMFx.LedCube.Effects
{
    public abstract class CubeEffect
    {
        public readonly NETMFx.LedCube.IO60P16.LedCube Cube;
        protected int Duration = 200;

        protected CubeEffect(NETMFx.LedCube.IO60P16.LedCube cube)
        {
            Cube = cube;
        }

        protected CubeEffect(NETMFx.LedCube.IO60P16.LedCube cube, int duration)
        {
            Cube = cube;
            Duration = duration;
        }

        public void Start()
        {
            Start(Duration);
        }

        public abstract void Start(int duration);
    }
}
