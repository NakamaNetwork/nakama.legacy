import { autoinject } from 'aurelia-framework';
import { GCRQueryService } from '../../services/query/gcr-query-service';
import { AlertService } from '../../services/alert-service';

@autoinject
export class GCREditPage {
    gcrQueryService: GCRQueryService;
    alertService: AlertService;

    title = 'GCR Editor';
    stages = [];
    units = [];
    loading;

    constructor(gcrQueryService: GCRQueryService, alertService: AlertService) {
        this.gcrQueryService = gcrQueryService;
        this.alertService = alertService;
    }

    activate(params) {
        this.reload();
    }

    reload() {
        if (this.gcrQueryService) {
            this.loading = true;
            this.gcrQueryService.get().then(x => {
                this.stages = x.stages;
                this.units = x.units;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }

    add(list) {
        list.push({ unitId: 1, stageId: 1002800, name: null, order: 99999999 });
    }

    delete(item, list) {
        var index = list.indexOf(item);
        if (index >= 0) {
            list.splice(index, 1);
        }
    }

    saveUnits() {
        this.loading = true;
        this.gcrQueryService.saveUnits(this.units).then(x => {
            this.alertService.success("Succesfully saved GCR units.");
            this.gcrQueryService.get().then(x => {
                this.units = x.units;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }).catch(e => {
            this.alertService.danger("Error saving GCR units.");
            this.loading = false;
        })
    }

    saveStages() {
        this.loading = true;
        this.gcrQueryService.saveStages(this.stages).then(x => {
            this.alertService.success("Succesfully saved GCR stages.");
            this.gcrQueryService.get().then(x => {
                this.stages = x.stages;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }).catch(e => {
            this.alertService.danger("Error saving GCR stages.");
            this.loading = false;
        })
    }
}