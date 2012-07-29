using Gadgeteer.Modules.GHIElectronics.IO60P16;

namespace NETMFx.LedCube.IO60P16
{
    /// <summary>
    /// A 3x3x3 LED Cube driver.
    /// </summary>
    public class LedCube3 : LedCube
    {
        public LedCube3(IO60P16Module parentModule, byte size, IOPin[] levelPins, IOPin[] ledPins, CubeOrientations orientation = CubeOrientations.ZPos) 
            : base(parentModule, size, levelPins, ledPins, orientation)
        {
        }
    }
}
