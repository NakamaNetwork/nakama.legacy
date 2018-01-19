import { autoinject, customElement } from 'aurelia-framework';
import { AlertService } from '../services/alert-service';

@autoinject
@customElement('alert-area')
export class AlertArea {
    private alertService: AlertService;

    constructor(alertService: AlertService) {
        this.alertService = alertService;
    }
}