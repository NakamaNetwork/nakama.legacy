import { autoinject } from 'aurelia-framework';
import { AlertService } from './alert-service';
import { IBoxDetailModel } from '../models/imported';
import { BoxQueryService } from './query/box-query-service';

@autoinject
export class BoxService {
    private boxQueryService: BoxQueryService;
    private alertService: AlertService;

    public currentBox: IBoxDetailModel = null;

    constructor(boxQueryService: BoxQueryService, alertService: AlertService) {
        this.boxQueryService = boxQueryService;
        this.alertService = alertService;
    }

    setBox(boxId: number) {
        this.boxQueryService.detail(boxId).then(x => {
            this.currentBox = x;
            this.alertService.info('You\'ve switched to box "' + x.name + '".');
        }).catch(x => {
            this.alertService.danger('There was an error switching boxes. Please try again in a moment.');
            this.currentBox = null;
        });
    }
}