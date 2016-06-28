using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapPlotter
{
    public class Edge
    {
        public Point PointA { get; set; }
        public Point PointB { get; set; }
        public double Distance { get; set; }
        public Guid ShapeId { get; set; }

        public class Point
        {

            public Point(double lat, double lng)
            {
                Lat = lat;
                Lng = lng;
            }

            public double Lat { get; set; }
            public double Lng { get; set; }


            public override string ToString()
            {
                return $"{Lat},{Lng}";
            }

            public override bool Equals(object obj)
            {
                return obj.ToString() == ToString();
            }

            public override int GetHashCode()
            {

                return ToString().GetHashCode();
            }
        }
    }
}
