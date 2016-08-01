using SharpMap;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace S57Map
{
    public class S52
    {
        // The color information listed below is from IHO S-52, Edition 6.1.1.  If you are going to change any of this stuff
        // find a copy of that and read it first.

        // the colorMap class is used to hold the actual RGB colors used to rendor a specific color and transparency level

        public class colorMap
        {
            public ColorName Name { get; internal set; }
            public string color { get; internal set; }
            public int R { get; internal set; }
            public int G { get; internal set; }
            public int B { get; internal set; }
            public float T { get; internal set; }
        }

        // according the the S-52 documents there should be 64 of these including the transpartent.  The document list 65
        // in one place including a RES03, and 62 in another.

        // do not changet the orders of these.  The order here and the order in the List, below, must be the same so we can use
        // these to index the list and not have to do a look up.  In other words if you add or delete from the enumerated type,
        // add or delete the corresponding entry in the list just below it.  They absolutely have to agree or you will render the
        // thw wrong color.

        public enum ColorName
        {
            ADINF, APLRT, ARPAT, BKAJ1, BKAJ2, CHBLK, CHBRN, CHCOR, CHGRD, CHGRF, CHGRN, CHMGD, CHMGF, CHRED, CHWHT,
            CHYLW, CSTLN, CURSR, DEPCN, DEPDW, DEPIT, DEPMD, DEPMS, DEPSC, DEPVS, DNGHL, ISDNG, LANDA, LANDF, LITGN,
            LITRD, LITYW, NINFO, NODTA, OUTLL, OUTLW, PLRTE, PSTRK, RADHI, RADLO, RES01, RES02, RESBL, RESGR, SCLBR,
            SHIPS, SNDG1, SNDG2, SYTRK, TRFCD, TRFCF, TRNSP, UIAFD, UIAFF, UIBCK, UIBDR, UINFB, UINFD, UINFF, UINFG,
            UINFM, UINFO, UINFR
        };

        // This is the "DAY" map.  I have not implemented the DUSK or NIGHT map.  However, if you choose to, do NOT reorder this
        // table, but rather, simply change the color assigned for each names used.  For example SHIPS changes from black during the
        // day to white at night.  The colorName assigned to each layer or symbol is invariant, meaning they do not change with Day,
        // Dusk, or Night mode.  What DOES change is the color assigned to them.

        // It might be easier to define the actual colors in another table, but they would need to be assigned unique names.
        // It is somewhat surprising to me that the IHO didn't do it like this, but again, I do not have their latest documents.
        // The color descriptive names, such as "blue-green", are not used anywhere so it would not be difficult to use variants of
        // these names to assign the RGB space.

        // Finally, the IHO describes colors in xyL form.  There are a lot of variables and math, to get through when converting
        // these to RGB.  These are my own best guesses of what they should look like.  At some point a color editor would be useful.

        public static List<colorMap> dayMap = new List<colorMap>()
        {
                new colorMap() { Name = ColorName.ADINF, color = "yellow",      R = 165,    G = 165,    B = 39,     T = 0.8f },
                new colorMap() { Name = ColorName.APLRT, color = "orange",      R = 227,    G = 128,    B = 57,     T = 0.8f },
                new colorMap() { Name = ColorName.ARPAT, color = "blue-green",  R = 45,     G = 168,    B = 121,    T = 0.8f },
                new colorMap() { Name = ColorName.BKAJ1, color = "grey",        R = 14,     G = 19,     B = 21,     T = 0.8f },
                new colorMap() { Name = ColorName.BKAJ2, color = "grey",        R = 28,     G = 35,     B = 39,     T = 0.8f },
                new colorMap() { Name = ColorName.CHBLK, color = "black",       R = 0,      G = 0,      B = 0,      T = 0.8f },
                new colorMap() { Name = ColorName.CHBRN, color = "brown",       R = 203,    G = 189,    B = 106,    T = 0.2f },
                new colorMap() { Name = ColorName.CHCOR, color = "orange",      R = 227,    G = 128,    B = 57,     T = 0.8f },
                new colorMap() { Name = ColorName.CHGRD, color = "grey",        R = 77,     G = 91,     B = 98,     T = 0.8f },
                new colorMap() { Name = ColorName.CHGRF, color = "grey",        R = 118,    G = 140,    B = 151,    T = 0.8f },
                new colorMap() { Name = ColorName.CHGRN, color = "green",       R = 82,     G = 232,    B = 59,     T = 0.8f },
                new colorMap() { Name = ColorName.CHMGD, color = "magenta",     R = 192,    G = 69,     B = 209,    T = 0.8f },
                new colorMap() { Name = ColorName.CHMGF, color = "magenta",     R = 203,    G = 169,    B = 249,    T = 0.8f },
                new colorMap() { Name = ColorName.CHRED, color = "red",         R = 234,    G = 84,     B = 113,    T = 0.8f },
                new colorMap() { Name = ColorName.CHWHT, color = "white",       R = 201,    G = 237,    B = 154,    T = 0.8f },
                new colorMap() { Name = ColorName.CHYLW, color = "yellow",      R = 225,    G = 225,    B = 57,     T = 0.8f },
                new colorMap() { Name = ColorName.CSTLN, color = "grey",        R = 77,     G = 91,     B = 98,     T = 0.8f },
                new colorMap() { Name = ColorName.CURSR, color = "orange",      R = 227,    G = 128,    B = 57,     T = 0.8f },
                new colorMap() { Name = ColorName.DEPCN, color = "grey",        R = 118,    G = 140,    B = 151,    T = 0.8f },
                new colorMap() { Name = ColorName.DEPDW, color = "white",       R = 201,    G = 237,    B = 154,    T = 0.2f },
                new colorMap() { Name = ColorName.DEPIT, color = "yellowgreen", R = 93,     G = 174,    B = 161,    T = 0.2f },
                new colorMap() { Name = ColorName.DEPMD, color = "blue",        R = 149,    G = 220,    B = 254,    T = 0.2f },
                new colorMap() { Name = ColorName.DEPMS, color = "blue",        R = 126,    G = 195,    B = 255,    T = 0.2f },
                new colorMap() { Name = ColorName.DEPSC, color = "grey",        R = 77,     G = 91,     B = 98,     T = 0.8f },
                new colorMap() { Name = ColorName.DEPVS, color = "blue",        R = 89,     G = 170,    B = 253,    T = 0.2f },
                new colorMap() { Name = ColorName.DNGHL, color = "red",         R = 234,    G = 84,     B = 113,    T = 0.8f },
                new colorMap() { Name = ColorName.ISDNG, color = "magenta",     R = 192,    G = 69,     B = 209,    T = 0.8f },
                new colorMap() { Name = ColorName.LANDA, color = "brown",       R = 191,    G = 190,    B = 143,    T = 0.8f },
                new colorMap() { Name = ColorName.LANDF, color = "brown",       R = 141,    G = 100,    B = 46,     T = 0.2f },
                new colorMap() { Name = ColorName.LITGN, color = "green",       R = 82,     G = 232,    B = 59,     T = 0.8f },
                new colorMap() { Name = ColorName.LITRD, color = "red",         R = 234,    G = 84,     B = 113,    T = 0.8f },
                new colorMap() { Name = ColorName.LITYW, color = "yellow",      R = 225,    G = 225,    B = 57,     T = 0.8f },
                new colorMap() { Name = ColorName.NINFO, color = "orange",      R = 227,    G = 128,    B = 57,     T = 0.8f },
                new colorMap() { Name = ColorName.NODTA, color = "grey",        R = 147,    G = 174,    B = 187,    T = 0.2f },
                new colorMap() { Name = ColorName.OUTLL, color = "brown",       R = 141,    G = 100,    B = 46,     T = 0.8f },
                new colorMap() { Name = ColorName.OUTLL, color = "brown",       R = 191,    G = 190,    B = 143,    T = 0.8f },
                new colorMap() { Name = ColorName.OUTLW, color = "black",       R = 0,      G = 0,      B = 0,      T = 0.8f },
                new colorMap() { Name = ColorName.PLRTE, color = "red",         R = 214,    G = 63,     B = 36,     T = 0.8f },
                new colorMap() { Name = ColorName.PSTRK, color = "black",       R = 0,      G = 0,      B = 0,      T = 0.8f },
                new colorMap() { Name = ColorName.RADHI, color = "green",       R = 82,     G = 232,    B = 59,     T = 0.8f },
                new colorMap() { Name = ColorName.RADLO, color = "green",       R = 47,     G = 142,    B = 32,     T = 0.8f },
                new colorMap() { Name = ColorName.RES01, color = "grey",        R = 118,    G = 140,    B = 151,    T = 0.8f },
                new colorMap() { Name = ColorName.RES02, color = "grey",        R = 118,    G = 140,    B = 151,    T = 0.8f },
                new colorMap() { Name = ColorName.RESBL, color = "blue",        R = 46,     G = 123,    B = 255,    T = 0.8f },
                new colorMap() { Name = ColorName.RESGR, color = "grey",        R = 118,    G = 140,    B = 151,    T = 0.8f },
                new colorMap() { Name = ColorName.SCLBR, color = "orange",      R = 227,    G = 128,    B = 57,     T = 0.8f },
                new colorMap() { Name = ColorName.SHIPS, color = "black",       R = 0,      G = 0,      B = 0,      T = 0.8f },
                new colorMap() { Name = ColorName.SNDG1, color = "grey",        R = 118,    G = 140,    B = 151,    T = 0.8f },
                new colorMap() { Name = ColorName.SNDG2, color = "black",       R = 0,      G = 0,      B = 0,      T = 0.8f },
                new colorMap() { Name = ColorName.SYTRK, color = "grey",        R = 118,    G = 140,    B = 151,    T = 0.8f },
                new colorMap() { Name = ColorName.TRFCD, color = "magenta",     R = 192,    G = 69,     B = 209,    T = 0.8f },
                new colorMap() { Name = ColorName.TRFCF, color = "magenta",     R = 203,    G = 169,    B = 249,    T = 0.8f },
                new colorMap() { Name = ColorName.TRNSP, color = "transparent", R = 75,     G = 100,    B = 75,     T = 0.8f },
                new colorMap() { Name = ColorName.UIAFD, color = "blue",        R = 89,     G = 170,    B = 253,    T = 0.8f },
                new colorMap() { Name = ColorName.UIAFF, color = "brown",       R = 191,    G = 190,    B = 143,    T = 0.8f },
                new colorMap() { Name = ColorName.UIBCK, color = "white",       R = 201,    G = 237,    B = 154,    T = 0.8f },
                new colorMap() { Name = ColorName.UIBDR, color = "grey",        R = 77,     G = 91,     B = 98,     T = 0.8f },
                new colorMap() { Name = ColorName.UINFB, color = "blue",        R = 46,     G = 123,    B = 255,    T = 0.8f },
                new colorMap() { Name = ColorName.UINFD, color = "black",       R = 0,      G = 0,      B = 0,      T = 0.8f },
                new colorMap() { Name = ColorName.UINFF, color = "grey",        R = 77,     G = 91,     B = 98,     T = 0.8f },
                new colorMap() { Name = ColorName.UINFG, color = "green",       R = 82,     G = 232,    B = 59,     T = 0.8f },
                new colorMap() { Name = ColorName.UINFM, color = "magenta",     R = 192,    G = 69,     B = 209,    T = 0.8f },
                new colorMap() { Name = ColorName.UINFO, color = "orange",      R = 227,    G = 128,    B = 57,     T = 0.8f },
                new colorMap() { Name = ColorName.UINFR, color = "red",         R = 234,    G = 84,     B = 113,    T = 0.8f }
        };

        // gets the RGBI of the given color from the color map
        // only here to shorten up the graphics calls since this is used each time
        static public Color colorMaptoRGBI(ColorName Name)
        {
            Color tColor;

            tColor = Color.FromArgb(Convert.ToInt32(dayMap[(int)Name].T * 255), dayMap[(int)Name].R, dayMap[(int)Name].G, dayMap[(int)Name].B);
            return tColor;
        }


        // Actual drawing is done here.  This needs a lot of work.
        // To Do:
        // Pen and Brush width should not be fixed.
        // Some way to rendor the symbol graphics
        // Assigning the outline colors instead of using black

        //static public VectorLayer Render(ColorName cName, ref VectorLayer vectorLayer, Ogr layerProvider, OSGeo.OGR.Feature feature, OSGeo.OGR.FieldDefn field, string WKTGeometry)
        static public VectorLayer Render(S57.S57Layer layerCollection)
        { 
            Map map = new Map();

            if (layerProvider.OgrGeometryTypeString.IndexOf("Polygon") > 0)
            {
                vectorLayer.Style.Fill = new SolidBrush(colorMaptoRGBI(cName));
                vectorLayer.Style.Outline = new Pen(colorMaptoRGBI(ColorName.CHBLK), 3.0f);
                vectorLayer.Style.EnableOutline = true;
            }
            else
            {
                vectorLayer.Style.Line = new Pen(colorMaptoRGBI(ColorName.CHBLK), 3.0f);
            }

            return vectorLayer;
        }
    }
}