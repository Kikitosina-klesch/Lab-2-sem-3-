using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;

namespace Lab_2_sem_3_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PointLatLng> points = new List<PointLatLng>();
        GMapMarker LastPath = null;
        GMapMarker LastArea = null;
        List<PointLatLng> findcoord= new List<PointLatLng>();
        PointLatLng fpoint;
        int count;
        List<CCar> cars = new List<CCar>();



        List<CMapOb> objs = new List<CMapOb>();
        //List<CMapOb> objs = new List<CMapOb>();

        public MainWindow()
        {
            InitializeComponent();
        }

        //void AddMark(string ToolType, string img)
        //{


        //    Map.Markers.Add(marker);

        //    points.Clear();
        //}

        //void AddPath()
        //{
        //    if (points.Count < 2)
        //        return;

        //    GMapMarker marker = new GMapRoute(points)
        //    {
        //        Shape = new Path()
        //        {
        //            Stroke = Brushes.DarkBlue, // цвет обводки
        //            Fill = Brushes.DarkBlue, // цвет заливки
        //            StrokeThickness = 4 // толщина обводки
        //        }
        //    };

        //    if (LastPath != null)
        //        Map.Markers.Remove(LastPath);

        //    LastPath = marker;
        //    Map.Markers.Add(marker);
        //}

        void AddArea()
        {
            if (points.Count < 3)
                return;

            GMapMarker marker = new GMapPolygon(points)
            {
                Shape = new Path
                {
                    Stroke = Brushes.Black, // стиль обводки
                    Fill = Brushes.Violet, // стиль заливки
                    Opacity = 0.7 // прозрачность
                }
            };

            if (LastArea != null)
                Map.Markers.Remove(LastArea);

            LastArea = marker;
            Map.Markers.Add(marker);
        }

        int getMinWayind()
        {
            int count0 = 0;
            int count1 = 0;
            double dist = 99999999999999999;

            foreach (CCar car in cars)
            {
                double dist1 = cars[count0].getDistance(objs[0].getFocus());
                if (dist1 < dist)
                {
                    dist = dist1;
                    count1 = count0;
                }

                count0++;
            }
            return count1;
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            // настройка доступа к данным
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // установка провайдера карт
            //Map.MapProvider = OpenStreetMapProvider.Instance;
            Map.MapProvider = YandexMapProvider.Instance;
            // установка зума карты
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;
            // установка фокуса карты
            Map.Position = new PointLatLng(55.012823, 82.950359);

            // настройка взаимодействия с картой
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;

        }

        private void Map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            //points.Add (Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y));
            if(Create.IsChecked == true)
            {
                switch (type.SelectedIndex)
                {
                    case 0:
                        //AddMark("Person", "bob.png");
                        objs.Add(new CPers(TName.Text, Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y), "bob.png"));
                     //   Map.Markers.Add(objs[objs.Count - 1].getMarker());
                        break;
                    case 1:
                        CCar car = new CCar(TName.Text, Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y), "cat.png", Map);
                        cars.Add(car);
                        objs.Add(car);
                        //objs.Add(cars[cars.Count - 1]);
                       //AddMark("Car", "cat.png");
                       //  Map.Markers.Add(objs[objs.Count - 1].getMarker());
                       
                       break;
                    case 2:
                        objs.Add(new CPlace(TName.Text, Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y), "smorc.png"));
                        //AddMark("Person", "smorc.png");
                        //Map.Markers.Add(objs[objs.Count - 1].getMarker());
                        ((CPers)objs[0]).setDestination(((CPlace)objs[cars.Count+1]).getFocus());
                        break;
                    case 3:
                        points.Add(Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y));
                        objs.Add(new CArea("r", points));


                        break;
                    //    //Map.Markers.Add(((CCar)objs[1]).moveTo(objs[0].getFocus()));
                    //    Map.Markers.Add(new CArea(TName.Text, Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y)));
                    //    break;

                    default:
                        MessageBox.Show("Unknow type!");
                        break;


                }
                Map.Markers.Clear();

                foreach (CMapOb pl in objs)
                    Map.Markers.Add(pl.getMarker());

            }
            else
            {
                res.Content = "";
                fpoint = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                //dispoint.Lat,dispoint.Lng
                //GeoCoordinate c1 = new GeoCoordinate(fpoint.Lat, fpoint.Lng);
                double[] dist = new double[objs.Count];
                int count = 0;

                foreach (CMapOb el in objs)
                {
                   
                    //PointLatLng spoint = el.getFocus();
                    //GeoCoordinate c2 = new GeoCoordinate(spoint.Lat, spoint.Lng);
                    
                    dist[count] = objs[count].getDistance(fpoint);
                    count++;
                    //res.Content += el.getTitle() + " : " + c2.GetDistanceTo(c1) + "\n";
                }
                for (int i = 0; i < objs.Count -1; i++)
                {
                    for (int j = 0; j < objs.Count - 1; j++)
                    {
                        if (dist[j] > dist[j + 1])
                        {
                            double z = dist[j];
                            dist[j] = dist[j + 1];
                            dist[j + 1] = z;
                        }
                    }
                        
                }
                for (int i = 0; i < objs.Count; i++)
                    res.Content += (i+1) + " : " + dist[i] + "\n";
            }

        }

        private void type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LastPath = null;
            
            points.Clear();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            LastPath = null;
            res.Content = "";
            List.Items.Clear();
            FName.Text = "";
            TName.Text = "";
            points.Clear();
            Map.Markers.Clear();
            objs.Clear();
            cars.Clear();
        }

        private void AddP_Click(object sender, RoutedEventArgs e)
        {
            //int count0 = 0;
            //int count1 = 0;
            //double dist = 99999999999999999;

            //foreach (CCar car in cars)
            //{         
            //    double dist1 = cars[count0].getDistance(objs[0].getFocus());
            //    if (dist1 < dist)
            //    {
            //        dist = dist1;
            //        count1 = count0;
            //    }

            //    count0++;
            //}

            ((CCar)objs[getMinWayind() + 1]).Arrived += ((CPers)objs[0]).CarArrived;
            ((CPers)objs[0]).passSeated += ((CCar)objs[getMinWayind() + 1]).passengerSeated;

            Map.Markers.Add((cars[getMinWayind()]).moveTo(objs[0].getFocus()));

            
            //objs.Add(new CRoute(TName.Text, points));

            //if (LastPath != null)
            //    Map.Markers.Remove(LastPath);

            //Map.Markers.Add(objs[objs.Count - 1].getMarker());
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            count = 0;
            List.Items.Clear();

            foreach (CMapOb el in objs)
            {
                
                if (FName.Text == el.getTitle())
                {
                    
                    List.Items.Add((count+1) + " : " + el.getTitle());
                    findcoord.Add (el.getFocus());
                    count++;
                }
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            PointLatLng p = findcoord[List.SelectedIndex];
            Map.Position = p;
        }
    }
}
