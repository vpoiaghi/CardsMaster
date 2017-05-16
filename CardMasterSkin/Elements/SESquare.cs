﻿using CardMasterSkin.Skins;

namespace CardMasterSkin.Elements
{
    public class SESquare : SERectangle
    {
        public SESquare(Skin skin, int size) : this(skin, 0, 0, size)
        { }

        public SESquare(Skin skin, int x, int y, int size) : base(skin, x, y, size, size)
        { }

    }
}