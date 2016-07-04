using SharpMap;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpMap_OGR_Test_v2
{
    internal class Render
    {
        static public Map S52(Ogr sourceProvider, float angle)
        {
            Random rnd = new Random(9);
            VectorLayer displayLayer;
            Ogr layerProvider;
            string layerName;
            Map map = new Map();

            for (Int32 i = sourceProvider.NumberOfLayers - 1; i >= 0; i--)
            {
                // open layer i
                layerProvider = new Ogr(sourceProvider.Filename, i);
                if (layerProvider.IsFeatureDataLayer)
                {
                    layerName = layerProvider.LayerName;
                    DebugUtil.WriteLine(string.Format("Layer {0}: {1}", i, layerName));

                    displayLayer = new VectorLayer(string.Format("Layer_{0}", layerName), layerProvider);
                    if (layerProvider.OgrGeometryTypeString.IndexOf("Polygon") > 0)
                    {
                        displayLayer.Style.Fill =
                            new SolidBrush(Color.FromArgb(150, Convert.ToInt32(rnd.NextDouble() * 255),
                                                          Convert.ToInt32(rnd.NextDouble() * 255),
                                                          Convert.ToInt32(rnd.NextDouble() * 255)));
                        displayLayer.Style.Outline =
                            new Pen(
                                Color.FromArgb(150, Convert.ToInt32(rnd.NextDouble() * 255),
                                               Convert.ToInt32(rnd.NextDouble() * 255),
                                               Convert.ToInt32(rnd.NextDouble() * 255)),
                                Convert.ToInt32(rnd.NextDouble() * 3));
                        displayLayer.Style.EnableOutline = true;
                    }
                    else
                    {
                        displayLayer.Style.Line =
                            new Pen(
                                Color.FromArgb(150, Convert.ToInt32(rnd.NextDouble() * 255),
                                               Convert.ToInt32(rnd.NextDouble() * 255), Convert.ToInt32(rnd.NextDouble() * 255)),
                                Convert.ToInt32(rnd.NextDouble() * 3));
                    }
                    map.Layers.Add(displayLayer);
                }
            }

            map.ZoomToExtents();

            Matrix mat = new Matrix();
            mat.RotateAt(angle, map.WorldToImage(map.Center));
            map.MapTransform = mat;

            return map;
        }
    }
}