using NETMFx.LedCube.IO60P16;using NETMFx.LedCube.IO60P16;

namespace NETMFx.LedCube.Effects
{
    public class TallyUp : CubeEffect
    {
        public TallyUp(NETMFx.LedCube.IO60P16.LedCube cube) : base(cube) { }
        public TallyUp(NETMFx.LedCube.IO60P16.LedCube cube, int duration) : base(cube, duration) { }

        public override void Start(int duration)
        {
            for (byte level = 0; level < Cube.Levels.Length; level++ )
            {
                for (byte led = 0; led < Cube.Leds.Length; led++ )
                {
                    Cube.Illuminate((byte)(led % Cube.Size), level, (byte)(led/Cube.Size), duration );
                }
            }
        }
    }
}
