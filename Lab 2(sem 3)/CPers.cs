using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lab_2_sem_3_
{
    public class CPers : CMapOb
    {
        PointLatLng location;
        GMapMarker marker;

        public event EventHandler passSeated;
        PointLatLng destination;

        // обработчик события прибытия такси

        public void CarArrived(object sender, EventArgs e)
        {
            passSeated?.Invoke(this, EventArgs.Empty);
            // TODO : сесть в машину
        }

        public void endOfJourney()
        {
            MessageBox.Show("Вы приЙехали!");
            // TODO : сесть в машину
        }
       
        public void setDestination(PointLatLng destination)
        {
            this.destination = destination;
        }

        public void setLocation(PointLatLng location)
        {
            this.location = location;
        }

        public PointLatLng getDestination()
        {
            return destination;
        }



        public CPers(string title, PointLatLng location, string img) : base(title)
        {
            this.location = location;

            marker = new GMapMarker(location)
            {
                Shape = new Image
                {
                    Width = 72, // ширина маркера
                    Height = 72, // высота маркера
                    ToolTip = title, // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/img/" + img)) // картинка
                }
            };
        }

        public override PointLatLng getFocus()
        {
            return location;
        }

        public override GMapMarker getMarker()
        {
            return marker;
        }

        public override double getDistance(PointLatLng location)
        {
            GeoCoordinate c1 = new GeoCoordinate(location.Lat, location.Lng);
            GeoCoordinate c2 = new GeoCoordinate(this.location.Lat, this.location.Lng);

            return c1.GetDistanceTo(c2);

            //double X = location.Lat;
            //double Y = location.Lng;
            //return Math.Sqrt(Math.Pow(otherpoint.Lat - X, 2) + Math.Pow(otherpoint.Lng - Y, 2));
        }
    }
}
