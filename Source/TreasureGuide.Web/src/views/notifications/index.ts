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

    notificationHandle;

    constructor(notificationQueryService: NotificationQueryService, alertService: AlertService) {
        this.notificationQueryService = notificationQueryService;
        this.alertService = alertService;
    }

    activate() {
        this.refresh();
        this.notificationHandle = setInterval(this.refresh, 30000);
    }

    deactivate() {
        clearInterval(this.notificationHandle);
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
            if (model) {
                this.notifications = this.notifications.filter(y => {
                    return y.id != model.id;
                });
            } else {
                this.refresh();
            }
        });
    }

    getLink(model: INotificationModel) {
        switch (model.eventType) {
            case NotificationEventType.TeamComment:
            case NotificationEventType.TeamVideo:
            case NotificationEventType.CommentReply:
                return '/teams/' + model.eventId + '/details';
            default:
                return null;
        }
    }

    getMessage(model: INotificationModel) {
        switch (model.eventType) {
            case NotificationEventType.TeamComment:
                return `A new comment was added to your team.`;
            case NotificationEventType.TeamVideo:
                return `A new video was added to your team.`;
            case NotificationEventType.CommentReply:
                return `Someone replied to one of your comments.`;
            default:
                return 'Unknown';
        }
    }
}