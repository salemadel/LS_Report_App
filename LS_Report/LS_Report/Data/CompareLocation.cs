using LS_Report.Interfaces;
using LS_Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LS_Report.Data
{
    public class CompareLocation
    {
        private double compare(List<double> coords, double[] client_coords)
        {
            double lat1 = coords[0];
            double lng1 = coords[1];
            double lat2 = client_coords[0];
            double lng2 = client_coords[1];
            double earthRadius = 6371; // in miles 3958.75, change to 6371 for kilometer output

            double dLat = ToRadian(lat2 - lat1);
            double dLng = ToRadian(lng2 - lng1);

            double sindLat = Math.Sin(dLat / 2);
            double sindLng = Math.Sin(dLng / 2);

            double a = Math.Pow(sindLat, 2) + Math.Pow(sindLng, 2)
                * Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2));

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double dist = earthRadius * c;

            return dist * 1000; // in meters
        }

        private double ToRadian(double degree)
        {
            return (Math.PI / 180) * degree;
        }

        private List<coords_Model> GeoLocationValidity(IDataStore dataStore, Client2 contact)
        {
            List<coords_Model> Coords_to_Return = new List<coords_Model>();

            if (contact.location.coordinates.Count() > 0)
            {
                if (contact.location.coordinates[0].HasValue && contact.location.coordinates[1].HasValue)
                {
                    double[] coords = new double[2];
                    coords[0] = (double)contact.location.coordinates[0];
                    coords[1] = (double)contact.location.coordinates[1];
                    var locations_list = dataStore.GetLocations().Where(i => i.Date.Date.Equals(DateTime.Now.Date)).ToList();
                    foreach (var coord in locations_list)
                    {
                        var dist = compare(coord.Coords, coords);
                        if (dist <= 25)
                        {
                            var location = new coords_Model
                            {
                                date = coord.Date,
                                dist = dist.ToString(),
                                lat = coord.Coords[0].ToString(),
                                lng = coord.Coords[1].ToString()
                            };
                            Coords_to_Return.Add(location);
                        }
                    }
                }
            }
            return Coords_to_Return;
        }

        public (bool, string, coords_Model, DateTime?, DateTime?) CompareLocationFunction(IDataStore dataStore, Client2 contact)
        {
            var coords_list = GeoLocationValidity(dataStore, contact);
            bool valid = false;
            string duration = "0";
            DateTime? startDate = null;
            DateTime? endDate = null;
            var coords = new coords_Model
            {
                lat = null,
                lng = null,
                dist = null
            };
            if (coords_list.Count > 0)
            {
                valid = true;
                var coords_list_orderbydate = coords_list.OrderBy(i => i.date).ToList();
                for (int i = 0; i < coords_list_orderbydate.Count - 1; i++)
                {
                    if ((i + 1) <= coords_list_orderbydate.Count - 1)
                    {
                        if ((coords_list_orderbydate[i + 1].date - coords_list_orderbydate[i].date).TotalSeconds <= 300)
                        {
                            duration = (Convert.ToDouble(duration) + (coords_list_orderbydate[i + 1].date - coords_list_orderbydate[i].date).TotalSeconds).ToString();
                        }
                    }
                }
                startDate = coords_list_orderbydate.First().date;
                endDate = coords_list_orderbydate.Last().date;
                coords.lat = coords_list.OrderBy(i => Convert.ToDouble(i.dist)).ToList().First().lat;
                coords.lng = coords_list.OrderBy(i => Convert.ToDouble(i.dist)).ToList().First().lng;
            }
            return (valid, duration, coords, startDate, endDate);
        }
    }

    public class coords_Model
    {
        public DateTime date { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string dist { get; set; }

        public coords_Model()
        {
            this.date = date;
            this.lat = lat;
            this.lng = lng;
            this.dist = dist;
        }
    }
}