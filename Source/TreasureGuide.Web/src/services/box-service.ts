import { autoinject } from 'aurelia-framework';
import { AlertService } from './alert-service';
import { IBoxDetailModel, IBoxUpdateModel } from '../models/imported';
import { BoxQueryService } from './query/box-query-service';
import { AccountService } from './account-service';
import { UserPreferenceType } from '../models/imported';

@autoinject
export class BoxService {
    private boxQueryService: BoxQueryService;
    private alertService: AlertService;
    private accountService: AccountService;

    public currentBox: IBoxDetailModel = null;
    private added: number[] = [];
    private removed: number[] = [];

    constructor(boxQueryService: BoxQueryService, alertService: AlertService, accountService: AccountService) {
        this.boxQueryService = boxQueryService;
        this.alertService = alertService;
        this.accountService = accountService;

        this.setBox(this.accountService.userProfile.userPreferences[UserPreferenceType.BoxId], true);
    }

    setBox(boxId, bypass: boolean = false) {
        this.save().then(x => {
            if (boxId) {
                var query;
                if (bypass) {
                    query = this.boxQueryService.detail(boxId);
                } else {
                    query = this.boxQueryService.focus(boxId);
                }
                query.then(y => {
                    this.currentBox = y;
                    if (!bypass) {
                        this.alertService.info('You\'ve switched to box "' + y.name + '".');
                    }
                    this.accountService.userProfile.userPreferences[UserPreferenceType.BoxId] = '' + boxId;
                }).catch(y => {
                    this.alertService.danger('There was an error loading your box. Please try again in a moment.');
                });
            }
        }).catch(x => {
            this.alertService.danger('There was an error switching boxes. Please try again in a moment.');
        });
    }

    private timer;

    queueSave() {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.timer = setTimeout(() => {
            this.save();
        }, 15000);
    }

    save() {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        var promise = new Promise<void>((resolve, reject) => {
            if (this.currentBox && (this.added.length > 0 || this.removed.length > 0)) {
                var model = <IBoxUpdateModel>{
                    id: this.currentBox.id,
                    added: this.added,
                    removed: this.removed
                }
                this.boxQueryService.update(model).then(x => {
                    this.added = [];
                    this.removed = [];
                    resolve();
                }).catch(x => {
                    reject(x);
                });
            } else {
                resolve();
            }
        });
        return promise;
    }

    toggle(unitId: number) {
        if (this.currentBox) {
            var index = this.currentBox.unitIds.indexOf(unitId);
            if (index > -1) {
                var addIndex = this.added.indexOf(unitId);
                if (addIndex > -1) {
                    this.added.slice(addIndex, 1);
                }
                this.removed.push(unitId);
                this.currentBox.unitIds.slice(index, 1);
            } else {
                var removeIndex = this.removed.indexOf(unitId);
                if (removeIndex > -1) {
                    this.removed.slice(removeIndex, 1);
                }
                this.added.push(unitId);
                this.currentBox.unitIds.push(unitId);
            }
            this.queueSave();
        }
    }
}