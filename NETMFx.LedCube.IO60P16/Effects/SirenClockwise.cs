using System;
using System.Threading;
using Microsoft.SPOT;
using NETMFx.LedCube.IO60P16;

namespace NETMFx.LedCube.Effects
{
    public class SirenClockwise : CubeEffect
    {
        public SirenClockwise(NETMFx.LedCube.IO60P16.LedCube cube) : base(cube) { }
        public SirenClockwise(NETMFx.LedCube.IO60P16.LedCube cube, int duration) : base(cube, duration) { }

        public override void Start(int duration)
        {
            int[] seq = { 0, 1, 2, 5, 8, 7, 6, 3 };
            Cube.Levels[0].Write(true);
            Cube.Levels[1].Write(true);
            Cube.Levels[2].Write(true);
            for (var i = 7; i >= 0; i--)
            {
                Cube.Leds[seq[i]].Write(true);
                Thread.Sleep(duration);
                Cube.Leds[seq[i]].Write(false);
            }
        }
    }
}
