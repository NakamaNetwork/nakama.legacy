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
        this.show(message, 4000, AlertType.Info);
    }

    warning(message: string) {
        this.show(message, 4000, AlertType.Warning);
    }

    danger(message: string) {
        this.show(message, 5000, AlertType.Danger);
    }

    success(message: string) {
        this.show(message, 3000, AlertType.Success);
    }

    show(message: string, duration: number, type: AlertType) {
        var id = ++this.count;
        var alert = new Alert(id, message, type);
        setTimeout(() => this.dismiss(id), duration);
        this.alerts.unshift(alert);
    }

    dismiss(id: number) {
        var alert = this.alerts.findIndex(x => x.id === id);
        if (alert >= 0) {
            this.alerts[alert].disappearing = true;
            setTimeout(() => {
                this.alerts.splice(alert, 1);
            }, 1000);
        }
    }
}

export class Alert {
    public id: number;
    public message: string;
    public type: AlertType;
    public disappearing: boolean;

    constructor(id: number, message: string, type: AlertType) {
        this.id = id;
        this.message = message;
        this.type = type;
        this.disappearing = false;
    }

    @computedFrom('type', 'disappearing')
    get class() {
        return '_' + AlertType[this.type].toLowerCase() + (this.disappearing ? ' disappearing' : '');
    }
}