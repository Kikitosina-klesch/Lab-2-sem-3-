using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2_sem_3_
{
    public abstract class CMapOb
    {
        string title;
        DateTime creationDate;

        public CMapOb(string title)
        {
            this.title = title;
            creationDate = DateTime.Now;
        }

        public string getTitle()
        {
            return title;
        }

        public DateTime getCreationDate()
        {
            return creationDate;
        }

        public abstract PointLatLng getFocus();

        public abstract GMapMarker getMarker();

        public abstract double getDistance(PointLatLng point);
    }
}
