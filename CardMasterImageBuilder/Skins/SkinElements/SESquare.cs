using CardMasterImageBuilder.Skins;
using System.IO;

namespace CardMasterImageBuilder.SkinElements
{
    public class SESquare : SERectangle
    {
        public SESquare(DirectoryInfo resourcesDirectory, int x, int y, int size, string comments) : base(resourcesDirectory, x, y, size, size, comments)
        { }

    }
}
