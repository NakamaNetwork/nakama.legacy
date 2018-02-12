import { computedFrom } from 'aurelia-framework';

export enum AlertType {
    Default,
    Info,
    Warning,
    Danger,
    Success
}

export class AlertService {
    private alerts: Alert[];
    private count: number;

    constructor() {
        this.alerts = [];
        this.count = 0;
    }

    info(message: string) {
        this.show(message, 4000, AlertType.Info, false);
    }

    warning(message: string) {
        this.show(message, 4000, AlertType.Warning, false);
    }

    danger(message: string) {
        this.show(message, 5000, AlertType.Danger, false);
    }

    success(message: string) {
        this.show(message, 3000, AlertType.Success, false);
    }

    news(message: string) {
        this.show(message, 20000, AlertType.Info, true);
    }

    reportError(response, message?: string) {
        message = message || 'An error has occurred. Please try again in a few moments.';
        response.text().then(x => {
            this.danger(x);
        }).catch(x => {
            this.danger(message);
        });
    }

    show(message: string, duration: number, type: AlertType, html: boolean) {
        var id = ++this.count;
        var alert = new Alert(id, message, type, html);
        setTimeout(() => this.dismiss(id), duration);
        this.alerts.unshift(alert);
    }

    dismiss(id: number) {
        var alert = this.alerts.find(x => x.id === id);
        if (alert) {
            alert.disappearing = true;
            setTimeout(() => {
                var index = this.alerts.findIndex(x => x.id === id);
                this.alerts.splice(index, 1);
            }, 1000);
        }
    }
}

export class Alert {
    public id: number;
    public message: string;
    public type: AlertType;
    public disappearing: boolean;
    public html: boolean;

    constructor(id: number, message: string, type: AlertType, html: boolean) {
        this.id = id;
        this.message = message;
        this.type = type;
        this.disappearing = false;
        this.html = html;
    }

    @computedFrom('type', 'disappearing')
    get class() {
        return '_' + AlertType[this.type].toLowerCase() + (this.disappearing ? ' disappearing' : '');
    }
}