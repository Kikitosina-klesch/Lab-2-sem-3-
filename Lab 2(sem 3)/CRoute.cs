using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab_2_sem_3_
{
    class CRoute : CMapOb
    {
        List<PointLatLng> locations;
        GMapMarker marker;

        public CRoute(string title, List<PointLatLng> locations) : base(title)
        {
            this.locations = locations;

            if (locations.Count < 2)
                return;

            marker = new GMapRoute(locations)
            {
                Shape = new Path()
                {
                    Stroke = Brushes.DarkBlue, // цвет обводки
                    Fill = Brushes.DarkBlue, // цвет заливки
                    StrokeThickness = 4 // толщина обводки
                }
            };            
        }

        public List<PointLatLng> getLocations()
        {
            return locations;
        }

        public override PointLatLng getFocus()
        {
            return locations[locations.Count/2];
        }

        public override GMapMarker getMarker()
        {
            return marker;
        }

        public override double getDistance(PointLatLng location)
        {
            GeoCoordinate c1 = new GeoCoordinate(location.Lat, location.Lng);
            GeoCoordinate c2 = new GeoCoordinate(this.locations[0].Lat, this.locations[0].Lng);

            return c1.GetDistanceTo(c2);

            //double X = location.Lat;
            //double Y = location.Lng;



            //return Math.Sqrt(Math.Pow(otherpoint.Lat - X, 2) + Math.Pow(otherpoint.Lng - Y, 2));
        }
    }
}
