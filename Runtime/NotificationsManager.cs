using System.Collections;
using Unity.Notifications;
using Unity.Notifications.Android;
using UnityEngine;

namespace NotificationsLibrary.Runtime
{
    public class NotificationsManager : MonoBehaviour
    {
        #region SERIALIZED_FEILDS

        [SerializeField] private AndroidNotificationChannel[] _channels;

        #endregion
        
        private static NotificationsManager _instance;
        public static NotificationsManager Instance => _instance;
        
        private Notification? _lastRespondedNotification;

        #region GETTERS

        public Notification? LastRespondedNotification => _lastRespondedNotification;

        #endregion

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeNotificationChannels();
            }
        }
        
        IEnumerator Start()
        {
            var request = NotificationCenter.RequestPermission();
            yield return new WaitUntil(() => request.Status == NotificationsPermissionStatus.RequestPending);
            Debug.Log("Permission result: " + request.Status);
            
            var query = NotificationCenter.QueryLastRespondedNotification();
            yield return new WaitUntil(() => query.State == QueryLastRespondedNotificationState.Pending);
            if (query.State == QueryLastRespondedNotificationState.HaveRespondedNotification)
            {
                _lastRespondedNotification = query.Notification;
            }
            else
            {
                _lastRespondedNotification = null;
            }
        }

        private void InitializeNotificationChannels()
        {
            NotificationCenterArgs args = NotificationCenterArgs.Default;
            args.PresentationOptions = NotificationPresentation.Badge | NotificationPresentation.Sound | NotificationPresentation.Vibrate;
            args.AndroidChannelName = "Default";
            args.AndroidChannelDescription = "Default notification channel";
            args.AndroidChannelId = "default";
            NotificationCenter.Initialize(args);
            
#if UNITY_ANDROID
            foreach (var channel in _channels) {
                AndroidNotificationCenter.RegisterNotificationChannel(channel);
            }
#endif
        }

        public void ScheduleNotification(BaseLocalNotification notification)
        {
            Notification notificationObj = new Notification();
            notificationObj.Identifier = notification.GetNotificationId();
            notificationObj.Badge = notification.GetNotificationBadge();
            notificationObj.Data = notification.GetNotificationExtraData();
            notificationObj.Title = notification.GetNotificationTitle();
            notificationObj.Group = notification.GetNotificationGroup();
            notificationObj.Text = notification.GetNotificationText();
            notificationObj.ShowInForeground = notification.CanShowNotificationInForeGround();
            notificationObj.IsGroupSummary = notification.IsGroupSummaryEnabled();

            NotificationCenter.ScheduleNotification<NotificationSchedule>(notificationObj,
                notification.GetNotificationCategory(), notification.GetNotificationSchedule());
        }

        public void CancelNotification(int id)
        {
            NotificationCenter.CancelScheduledNotification(id);
        }

        public void CancelAllNotifications()
        {
            NotificationCenter.CancelAllScheduledNotifications();
        }

        public void RemoveNotificationFromTray(int id)
        {
            NotificationCenter.CancelDeliveredNotification(id);
        }

        public void RemoveAllNotificationsFromTray()
        {
            NotificationCenter.CancelAllDeliveredNotifications();
        }

        public NotificationsPermissionStatus GetNotificationsPermissionStatus()
        {
           NotificationsPermissionRequest request = NotificationCenter.RequestPermission();
           return request.Status;
        }
    }
}

