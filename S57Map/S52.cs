using SharpMap;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System;
using System.Drawing;

namespace S57Map
{
    internal class S52
    {
        static public VectorLayer Render(ref VectorLayer vectorLayer, Ogr layerProvider, OSGeo.OGR.Feature feature, OSGeo.OGR.FieldDefn field, string WKTGeometry)
        {
            Random rnd = new Random(9);
            // return vectorLayer;

            Map map = new Map();

            if (layerProvider.OgrGeometryTypeString.IndexOf("Polygon") > 0)
            {
                vectorLayer.Style.Fill =
                    new SolidBrush(Color.FromArgb(150, Convert.ToInt32(rnd.NextDouble() * 255),
                                                  Convert.ToInt32(rnd.NextDouble() * 255),
                                                  Convert.ToInt32(rnd.NextDouble() * 255)));
                vectorLayer.Style.Outline =
                    new Pen(
                        Color.FromArgb(150, Convert.ToInt32(rnd.NextDouble() * 255),
                                       Convert.ToInt32(rnd.NextDouble() * 255),
                                       Convert.ToInt32(rnd.NextDouble() * 255)),
                        Convert.ToInt32(rnd.NextDouble() * 3));
                vectorLayer.Style.EnableOutline = true;
            }
            else
            {
                vectorLayer.Style.Line =
                    new Pen(
                        Color.FromArgb(150, Convert.ToInt32(rnd.NextDouble() * 255),
                                       Convert.ToInt32(rnd.NextDouble() * 255), Convert.ToInt32(rnd.NextDouble() * 255)),
                        Convert.ToInt32(rnd.NextDouble() * 3));
            }

            return vectorLayer;
        }
    }
}