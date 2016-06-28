using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapPlotter.UI.Web.Models
{
    public class ShapeGroupModel
    {
        public List<ShapeModel> Shapes { get; set; }

        public class ShapeModel
        {
            public List<PointModel> Points { get; set; }


        }

        public class PointModel
        {

            public double Lat { get; set; }
            public double Lng { get; set; }
        }
    }
}
