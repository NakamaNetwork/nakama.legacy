import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';

@autoinject
export class NotificationQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }

    get() {
        return this.http.get('/api/notifications');
    }

    count() {
        return this.http.get('/api/notifications/count');
    }

    acknowledge(id: number) {
        return this.http.delete('/api/notifications/' + id);
    }

    notificationCount: number;

    refreshNotifications() {
        this.count().then(x => {
            this.notificationCount = x;
        });
    }
}