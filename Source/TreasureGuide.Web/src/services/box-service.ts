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

        if (this.accountService.userProfile) {
            this.setBox(this.accountService.userProfile.userPreferences[UserPreferenceType.BoxId], true);
        }
    }

    setBox(boxId, bypass: boolean = false) {
        this.save(false).then(x => {
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

    queueSave(messages: boolean = true) {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.timer = setTimeout(() => {
            this.save(messages);
        }, 10000);
    }

    get dirty() {
        return this.currentBox && (this.added.length > 0 || this.removed.length > 0);
    }

    save(messages: boolean = true) {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        var promise = new Promise<void>((resolve, reject) => {
            if (this.dirty) {
                var model = <IBoxUpdateModel>{
                    id: this.currentBox.id,
                    added: this.added,
                    removed: this.removed
                }
                this.boxQueryService.update(model).then(x => {
                    this.added = [];
                    this.removed = [];
                    resolve();
                    this.alertService.success('Your box units have been saved!');
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
                this.added = this.added.filter(x => x !== unitId);
                this.currentBox.unitIds = this.currentBox.unitIds.filter(x => x !== unitId);
                if (this.removed.indexOf(unitId) === -1) {
                    this.removed.push(unitId);
                }
            } else {
                this.removed.filter(x => x !== unitId);
                if (this.added.indexOf(unitId) === -1) {
                    this.added.push(unitId);
                }
                this.currentBox.unitIds.push(unitId);
            }
            this.queueSave();
        }
    }
}