using System;
using Unity.Notifications;

namespace NotificationsLibrary.Runtime
{
    public abstract class BaseLocalNotification
    {
        public abstract int? GetNotificationId();

        public virtual int GetNotificationBadge()
        {
            return 0;
        }

        public abstract string GetNotificationExtraData();

        public abstract string GetNotificationTitle();

        public virtual string GetNotificationGroup()
        {
            return String.Empty;
        }

        public abstract string GetNotificationText();

        public virtual bool CanShowNotificationInForeGround()
        {
            return false;
        }

        public virtual bool IsGroupSummaryEnabled()
        {
            return false;
        }

        public abstract string GetNotificationCategory();

        /// <summary>
        /// Set the NotificationSchedule in form of either <see cref="NotificationDateTimeSchedule"/> or <see cref="NotificationIntervalSchedule"/>
        /// Or Create Custom Schedule by implementing <see cref="NotificationSchedule"/>
        /// </summary>
        /// <returns></returns>
        public abstract NotificationSchedule GetNotificationSchedule();
    }
    
}
