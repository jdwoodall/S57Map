using SharpMap;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System.Drawing.Drawing2D;

namespace SharpMap_OGR_Test_v2
{
    internal class RenderS57
    {
        // display the current tile.  This does not work at the moment.
        public static Map Render(Ogr provider, float angle)
        {
            //Initialize a new map of size 'imagesize'
            Map map = new Map();

            // Layer Name
            string layerName;

            // temporary layer to build layer image on
            VectorLayer thisVectorLayer;

            map.ZoomToExtents();

            // don't rotate if we don't need to?????
            if (angle != 0.0f)
            {
                Matrix mat = new Matrix();
                mat.RotateAt(angle, map.WorldToImage(map.Center));
                map.MapTransform = mat;
            }

            return map;
        }
    }
}