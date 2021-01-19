using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lab_2_sem_3_
{
    public class CCar : CMapOb
    {
        PointLatLng location;
        GMapMarker marker;

        CRoute route;
        CPers pers;

        GMapControl gMap;

        // событие прибытия
        public event EventHandler Arrived;
        

        public CCar(string title, PointLatLng location, string img, GMapControl Map): base (title)
        {
            this.location = location;
            this.gMap = Map;

            marker = new GMapMarker(location)
            {
                Shape = new Image
                {
                    Width = 42, // ширина маркера
                    Height = 42, // высота маркера
                    ToolTip = title, // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/img/" + img)) // картинка
                }
            };
        }

        public void setPerson(CPers pers)
        {
            this.pers = pers;
        }

        public override PointLatLng getFocus()
        {
            return location;
        }

        public override GMapMarker getMarker()
        {
            return marker;
        }

        public void passengerSeated(object sender, EventArgs e)
        {
            pers = (CPers)sender;

            Application.Current.Dispatcher.Invoke(delegate {
                // gMap.Markers.Add(moveTo(pers.getDestination()));
                (Application.Current.MainWindow as MainWindow).Progress.Value = 0;
            });


            Application.Current.Dispatcher.Invoke(delegate {
                gMap.Markers.Add(moveTo(pers.getDestination()));
            });

            //Application.Current.Dispatcher.Invoke((Application.Current.MainWindow as MainWindow).Progress.Value = 0;);
        }

       
        // метод перемещения по маршруту
        private void MoveByRoute()
        {
            double pointss = 100 / route.getLocations().Count;
            foreach (var point in route.getLocations())
            {
                this.location = point;                
                Application.Current.Dispatcher.Invoke(delegate
                {
                    this.location = point;
                    marker.Position = point;
                    gMap.Position = point;
                    (Application.Current.MainWindow as MainWindow).Progress.Value += pointss + 1;
                    if (pers != null)
                    {
                        pers.setLocation(point);
                        pers.getMarker().Position = point;
                    }

                });
                Thread.Sleep(300);
            }
            // отправка события о прибытии после достижения последней точки маршрута
            if (pers == null)
                Arrived?.Invoke(this, null);
            else
            {
                pers.endOfJourney();
                pers = null;
            }
        }

        public GMapMarker moveTo(PointLatLng endLocation)
        {
            // провайдер навигации
            RoutingProvider routingProvider = GMapProviders.OpenStreetMap;
            // определение маршрута
            MapRoute route = routingProvider.GetRoute(
            location, // начальная точка маршрута
            endLocation, // конечная точка маршрута
            false, // поиск по шоссе (false - включен)
            false, // режим пешехода (false - выключен)
            (int)15);

            // получение точек маршрута
            List<PointLatLng> routePoints = route.Points;
            
            this.route = new CRoute("r", routePoints);

            Thread newThread = new Thread(new ThreadStart(MoveByRoute));
            newThread.Start();

            return this.route.getMarker();
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
