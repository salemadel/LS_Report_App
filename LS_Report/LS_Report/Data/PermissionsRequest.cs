using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LS_Report.Data
{
    public class PermissionsRequest
    {
        public async Task<PermissionStatus> Check_permissions(string type)
        {
            PermissionStatus status = PermissionStatus.Unknown;
            if (type == "Location")
            {
                status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    var results = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                    if (results == PermissionStatus.Granted)
                        status = PermissionStatus.Granted;
                }
            }
            if (type == "Storage")
            {
                status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                status = (status == PermissionStatus.Granted) ? await Permissions.CheckStatusAsync<Permissions.StorageWrite>() : PermissionStatus.Denied;
                if (status != PermissionStatus.Granted)
                {
                    var results = await Permissions.RequestAsync<Permissions.StorageRead>();
                    results = (results == PermissionStatus.Granted) ? await Permissions.RequestAsync<Permissions.StorageWrite>() : PermissionStatus.Denied;
                    if (results == PermissionStatus.Granted)
                        status = PermissionStatus.Granted;
                }
            }
            return status;
        }
    }
}