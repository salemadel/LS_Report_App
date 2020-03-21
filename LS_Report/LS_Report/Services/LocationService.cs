using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.Services
{
    public class LocationService
    {
        private IDataStore DataStore { get; set; }
        private PermissionsRequest permissions { get; set; }

        public LocationService(IDataStore dataStore)
        {
            DataStore = dataStore;
            permissions = new PermissionsRequest();
        }

        public async Task GetLocationAsync()
        {
            try
            {
                Position position = null;
                List<double> coords = new List<double>();
                string LogguedIn = null;
                if (CrossSecureStorage.Current.HasKey("LogguedIn"))
                {
                    LogguedIn = CrossSecureStorage.Current.GetValue("LogguedIn");
                }
                if (LogguedIn == "True")
                {
                    try
                    {
                        if (await permissions.Check_permissions("Location") == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            var locator = CrossGeolocator.Current;

                            locator.DesiredAccuracy = 100;

                            if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                            {
                                position = null;
                                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(9), null, true);
                                if (position != null)
                                {
                                    if (coords.Count > 0)
                                    {
                                        coords.RemoveAt(0);
                                        coords.RemoveAt(1);
                                    }
                                    coords.Insert(0, position.Latitude);
                                    coords.Insert(1, position.Longitude);
                                    var location = new LocationsBackGround_Model
                                    {
                                        Coords = coords,
                                        Date = DateTime.Now,
                                    };
                                    DataStore.AddLocation(location);
                                }
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DependencyService.Get<ISettingService>().OpenSettings();
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}