using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlybird.Components
{
    public enum BirdSkin
    {
        common,
        amogus,
        //doodle,
        upsideDown,
        formal,
        //rich,
        patriotic,
    }
    public static class BirdSkinInfo
    {
        public static Dictionary<BirdSkin, string> SkinNames = new Dictionary<BirdSkin, string>
        { 
            { BirdSkin.common, "common" },
            { BirdSkin.amogus, "amogus" },
            { BirdSkin.formal, "formal" },
            { BirdSkin.patriotic, "patriotic" },
            { BirdSkin.upsideDown, "upside-down" },
        };

        public static Dictionary<BirdSkin, int> SkinPrices = new Dictionary<BirdSkin, int>
        {
            { BirdSkin.common, 0 },
            { BirdSkin.amogus, 15 },
            { BirdSkin.formal, 69 },
            { BirdSkin.patriotic, 100 },
            { BirdSkin.upsideDown, 5 },
        };
    }
}
