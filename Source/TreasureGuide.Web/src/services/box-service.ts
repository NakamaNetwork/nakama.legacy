import { autoinject, computedFrom } from 'aurelia-framework';
import { AlertService } from './alert-service';
import { IBoxDetailModel, IBoxUpdateModel } from '../models/imported';
import { BoxQueryService } from './query/box-query-service';
import { AccountService } from './account-service';
import { UserPreferenceType } from '../models/imported';
import { RoleConstants } from '../models/imported';
import { BoxConstants } from '../models/imported';
import { BoxDetailModel } from './query/box-query-service';
import { IBoxUnitDetailModel } from '../models/imported';

@autoinject
export class BoxService {
    private boxQueryService: BoxQueryService;
    private alertService: AlertService;
    private accountService: AccountService;

    public currentBox: BoxDetailModel = null;
    private added: number[] = [];
    private removed: number[] = [];

    constructor(boxQueryService: BoxQueryService, alertService: AlertService, accountService: AccountService) {
        this.boxQueryService = boxQueryService;
        this.alertService = alertService;
        this.accountService = accountService;

        if (this.accountService.userProfile) {
            var boxId = this.accountService.userProfile.userPreferences[UserPreferenceType.BoxId];
            if (boxId) {
                this.setBox(boxId, true);
            }
        }
    }

    get boxCount() {
        return this.accountService.userProfile.boxCount;
    }

    set boxCount(value) {
        this.accountService.userProfile.boxCount = value;
    }

    @computedFrom('accountService.userProfile', 'accountService.userProfile.roles.length')
    get boxLimit() {
        if (this.accountService.isInRoles([RoleConstants.Administrator, RoleConstants.Moderator])) {
            return 999;
        } else if (this.accountService.isInRoles(RoleConstants.MultiBoxUser)) {
            return BoxConstants.MultiBoxUserLimit;
        } else if (this.accountService.isInRoles(RoleConstants.BoxUser)) {
            return BoxConstants.BoxUserLimit;
        }
        return 0;
    }

    @computedFrom('boxCount', 'boxLimit')
    get reachedLimit() {
        return this.boxCount >= this.boxLimit;
    }

    setBox(boxId, bypass: boolean = false) {
        if (boxId && this.accountService.isInRoles(RoleConstants.BoxUser)) {
            return this.doSave(false).then(x => {
                if (boxId) {
                    var query;
                    if (bypass) {
                        query = this.boxQueryService.detail(boxId);
                    } else {
                        query = this.boxQueryService.focus(boxId);
                    }
                    return query.then(y => {
                        this.currentBox = Object.assign(new BoxDetailModel(), y);
                        if (!bypass) {
                            this.alertService.info('You\'ve switched to box "' + y.name + '".');
                        }
                        this.accountService.userProfile.userPreferences[UserPreferenceType.BoxId] = '' + boxId;
                    }).catch(response => this.alertService.reportError(response, 'There was an error loading your box. Please try again in a few moments.'));
                }
            }).catch(response => this.alertService.reportError(response, 'There was an error switching boxes. Please try again in a few moments.'));
        }
        return this.boxQueryService.focus(null).then(y => {
            this.currentBox = null;
            this.alertService.info('Box closed. You can open another via the Box menu.');
        }).catch(response => this.alertService.reportError(response, 'There was an error closing your box. Please try again in a few moments.'));
    }

    private timer;

    queueSave(messages: boolean = true, delay: number = 5000) {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.timer = setTimeout(() => {
            this.doSave(messages);
        }, delay);
    }

    save(messages: boolean = true) {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.doSave(messages);
    }

    @computedFrom('currentBox', 'added.length', 'removed.length')
    get dirty() {
        return this.currentBox && (this.added.length > 0 || this.removed.length > 0);
    }

    doSave(messages: boolean = true) {
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

    toggle(unitId: number, box: BoxDetailModel) {
        var myBox = !box;
        box = box || this.currentBox;
        if (box) {
            var index = box.unitIds.indexOf(unitId);
            if (index > -1) {
                if (myBox) {
                    this.added = this.added.filter(x => x !== unitId);
                    if (this.removed.indexOf(unitId) === -1) {
                        this.removed.push(unitId);
                    }
                }
                box.boxUnits = box.boxUnits.filter(x => x.unitId !== unitId);
            } else {
                if (myBox) {
                    this.removed.filter(x => x !== unitId);
                    if (this.added.indexOf(unitId) === -1) {
                        this.added.push(unitId);
                    }
                }
                box.boxUnits.push(<IBoxUnitDetailModel>{ unitId: unitId });
            };
        }
        if (myBox) {
            this.queueSave();
        }
    }
}