using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using LS_Report.Data;
using LS_Report.Services;
using System.Threading;
using System.Threading.Tasks;

namespace LS_Report.Droid.Services
{
    [Service]
    internal class BackGroundService : Service
    {
        public const int FORSERVICE_NOTIFICATION_ID = 10000;
        public const string MAIN_ACTIVITY_ACTION = "Main_activity";
        public const string PUT_EXTRA = "has_service_been_started";
        public CancellationTokenSource _cts = new CancellationTokenSource();
        public static bool isStarted = false;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // From shared code or in your PCL
            base.OnStartCommand(intent, flags, startId);

            if (isStarted == false)
            {
                registerForService();
                BuildIntentToShowMainActivity();

                isStarted = true;
                var datastore = new DataStore();
                Task LocationTaskt = new Task(async () =>
                {
                    var _locationService = new LocationService(datastore);
                    while (true)
                    {
                        await _locationService.GetLocationAsync();
                        await Task.Delay(10000);
                    }
                });
                LocationTaskt.Start();
                Task UploadDataTask = new Task(async () =>
                {
                    var _UploadDataService = new UploadDataService(datastore);
                    while (true)
                    {
                        await _UploadDataService.UploadDataAsync();
                        await Task.Delay(120000);
                    }
                });
                UploadDataTask.Start();
                Task MessagesListningTask = new Task(async () =>
                {
                    var _MessageListning = new MessagesListnerService(datastore);
                    while (true)
                    {
                        await _MessageListning.Listner();
                        await Task.Delay(320000);
                    }
                });
                MessagesListningTask.Start();
            }
            return StartCommandResult.Sticky;
        }

        private void registerForService()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel("my_chanel_2", "My chanel", NotificationImportance.High)
                {
                    Description = "Foreground Service Channel",
                    LockscreenVisibility = NotificationVisibility.Public
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
                var notification = new Notification.Builder(this, "my_chanel_2")
                    .SetContentTitle("LS Report")
                    .SetContentText("LS Report est en cours d'execution...")
                    //  .SetSmallIcon(Resource.Drawable.icon)
                    .SetContentIntent(BuildIntentToShowMainActivity())
                    .SetOngoing(true)
                    //   .AddAction(BuildRestartTimerAction())
                    //  .AddAction(BuildStopServiceAction())
                    .Build();

                // Enlist this instance of the service as a foreground service, MUST CALL IN < 5 SECONDS ON RUNTIME

                StartForeground(FORSERVICE_NOTIFICATION_ID, notification);
            }
            else
            {
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetContentTitle("LS Report")
                .SetContentText("LS Report est en cours d'execution...")
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetAutoCancel(true);

                Notification notification = builder.Build();

                StartForeground(1, notification);
            }
        }

        private PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetAction(Intent.ActionMain);
            notificationIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.BroughtToFront | ActivityFlags.ReorderToFront);
            notificationIntent.PutExtra(PUT_EXTRA, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);
            return pendingIntent;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            // registerForService();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            isStarted = false;
        }
    }
}