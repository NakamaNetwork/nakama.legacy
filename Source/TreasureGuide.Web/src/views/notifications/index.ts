import { autoinject } from 'aurelia-framework';
import { NotificationQueryService } from '../../services/query/notification-query-service';
import { INotificationModel, NotificationEventType } from '../../models/imported';
import { AlertService } from '../../services/alert-service';

@autoinject
export class NotificationsPage {
    notificationQueryService: NotificationQueryService;
    alertService: AlertService;

    loading: boolean;
    notifications: INotificationModel[] = [];

    constructor(notificationQueryService: NotificationQueryService, alertService: AlertService) {
        this.notificationQueryService = notificationQueryService;
        this.alertService = alertService;
    }

    activate() {
        this.refresh();
        setInterval(this.refresh, 30000);
    }

    refresh() {
        this.loading = true;
        this.notificationQueryService.get().then(x => {
            this.notifications = x;
            this.notificationQueryService.notificationCount = x.length;
            this.loading = false;
        });
    }

    acknowledge(model: INotificationModel) {
        var query;
        if (model) {
            query = this.notificationQueryService.acknowledge(model.id);
        } else {
            query = this.notificationQueryService.acknowledge(null);
        }
        query.then(x => {
            this.alertService.success('Notification dismissed.');
            this.refresh();
        });
    }

    getLink(model: INotificationModel) {
        switch (model.eventType) {
            case NotificationEventType.TeamComment:
            case NotificationEventType.TeamVideo:
                return '/teams/' + model.eventId + '/details';
            case NotificationEventType.CommentReply:
                return '/teams/' + model.extraInfo + '/details';
            default:
                return null;
        }
    }

    getMessage(model: INotificationModel) {
        switch (model.eventType) {
            case NotificationEventType.TeamComment:
                return `"${model.triggerUserName}" commented on your team "${model.eventInfo}".`;
            case NotificationEventType.TeamVideo:
                return `"${model.triggerUserName}" added a video to your team "${model.eventInfo}".`;
            case NotificationEventType.CommentReply:
                return `"${model.triggerUserName}" replied to your comment "${model.eventInfo}".`;
            default:
                return 'Unknown';
        }
    }
}