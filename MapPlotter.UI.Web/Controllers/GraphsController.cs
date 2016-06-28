using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MapPlotter.UI.Web.Models;

namespace MapPlotter.UI.Web.Controllers
{
    public class GraphsController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Demo()
        {

            return View();
        }



        public JsonResult GetShapes()
        {
            var graph = new ShapeGroupModel();

            var edges = ReadFile("graph.txt");

            graph.Shapes = edges.GroupBy(p => p.ShapeId).Select(s =>
                new ShapeGroupModel.ShapeModel()
                {
                    Points = s.SelectMany(ep=>new List<Edge.Point>() { ep.PointA, ep.PointB}).Distinct().Select(pm=>new ShapeGroupModel.PointModel()
                    {
                        Lat = pm.Lat,
                        Lng = pm.Lng
                    }).ToList()
                }).ToList();
     

            return Json(graph, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGraph()
        {
            var graph = new GraphModel();

            var edges = ReadFile("graph.txt");

            graph.Edges = edges;


            return Json(graph, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveGraph(ShapeGroupModel model)
        {

            var existingEdges = new List<Edge>();
            var existingPoints = new List<Edge.Point>();

            foreach (var shape in model.Shapes)
            {

                int index = 0;
                Edge currentEdge = null;
                var shapeId = Guid.NewGuid();
                foreach (var point in shape.Points)
                {
                    // need we snap first and last points to the nearest vertice, within 20km
                   
                    var ordered = existingPoints.Select(p => new
                    {
                        Point = p,
                        Distance = GetDistance(p.Lat, p.Lng, point.Lat, point.Lng)
                    }).OrderBy(p => p.Distance).ToList();

                    var snapIn = ordered.FirstOrDefault(p => p.Distance < 5000);

                    if (snapIn != null)
                    {
                        point.Lat = snapIn.Point.Lat;
                        point.Lng = snapIn.Point.Lng;
                    }
                    

                    var edge = new Edge.Point(point.Lat, point.Lng);
                    existingPoints.Add(edge);

                    if (currentEdge == null)
                    {
                        currentEdge = new Edge()
                        {
                            PointA = edge,
                            ShapeId = shapeId
                        };
                    }
                    else
                    {
                        currentEdge.PointB = edge;
                        currentEdge.Distance = GetDistance(currentEdge.PointA.Lat, currentEdge.PointA.Lng,
                            currentEdge.PointB.Lat, currentEdge.PointB.Lng);
                        existingEdges.Add(currentEdge);
                        // We start up a new edge
                        currentEdge = new Edge()
                        {
                            PointA = edge,
                            ShapeId = shapeId
                        };
                    }
                    index++;
                }

            }


            SaveFile("graph.txt", existingEdges);

            return Json(new {success = true});
        }



        public void SaveFile(string fileName, List<Edge> edges)
        {
            using (var sw = new StreamWriter(Server.MapPath($"~/App_Data/Shapes/{fileName}"),false))
            {
                foreach (var edge in edges)
                {
                    sw.WriteLine($"{edge.PointA.Lat},{edge.PointA.Lng},{edge.PointB.Lat},{edge.PointB.Lng},{edge.Distance},{edge.ShapeId}");
                }
            }
        }


        public List<Edge> ReadFile(string fileName)
        {
            using (var sr = new StreamReader(Server.MapPath($"~/App_Data/Shapes/{fileName}")))
            {
                var edges = new List<Edge>();

                while (!sr.EndOfStream)
                {
                    var edgeStr = sr.ReadLine();
                    if (edgeStr != null)
                    {
                        var split = edgeStr.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);

                        if (split.Length == 6)
                        {
                            var pointA = new Edge.Point(double.Parse(split[0]), double.Parse(split[1]));
                            var pointB = new Edge.Point(double.Parse(split[2]), double.Parse(split[3]));
                            var distance = double.Parse(split[4]);
                            edges.Add(new Edge()
                            {
                                PointA = pointA,
                                PointB = pointB,
                                Distance = distance,
                                ShapeId = Guid.Parse(split[5])
                            });
                        }
                        else
                        {
                            throw new Exception("File Corrupt");
                        }
                    }
                }

                return edges;
            }
        }


        public List<Edge.Point> GetAllPoints(List<Edge> edges)
        {
            var retVal = new List<Edge.Point>();

            edges.ForEach(f =>
            {
                retVal.Add(f.PointA);
                retVal.Add(f.PointB);
            });


            return retVal;
            
        }

        private double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            int srid = 4326;
            string wkt = $"POINT({lat1} {lng1})";
            var geoPointA = DbGeography.PointFromText($"POINT({lat1} {lng1})", srid);
            var geoPointB = DbGeography.PointFromText($"POINT({lat2} {lng2})", srid);


            var distance = geoPointA.Distance(geoPointB);

            return distance ?? 0;

        }




       

      
    }

}