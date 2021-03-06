﻿import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitQueryService } from '../services/query/unit-query-service';
import { DialogService } from 'aurelia-dialog';
import { NumberHelper } from '../tools/number-helper';
import { UnitPicker } from './dialogs/unit-picker';
import { UnitPickerParams } from './dialogs/unit-picker';
import { IUnitStubModel } from '../models/imported';
import { UnitType } from '../models/imported';
import { UnitClass } from '../models/imported';
import { UnitRole, IBoxDetailModel } from '../models/imported';
import { StringHelper } from '../tools/string-helper';
import { BoxService } from '../services/box-service';
import { BoxDetailModel } from '../services/query/box-query-service';

@autoinject
@customElement('unit-display')
export class UnitDisplay {
    private unitQueryService: UnitQueryService;
    private dialogService: DialogService;
    private boxService: BoxService;
    private element: Element;

    @bindable unitId: number;
    @bindable model: any;
    @bindable editable: boolean;
    @bindable allowGenerics: boolean;
    @bindable info: boolean;
    @bindable tooltip: boolean;
    @bindable showBox: boolean;
    @bindable showBoxFlags: boolean;
    @bindable box: BoxDetailModel;
    @bindable editorKey: string;
    @bindable icon: string;

    inBox: boolean;

    constructor(unitQueryService: UnitQueryService, dialogService: DialogService, boxService: BoxService, element: Element) {
        this.unitQueryService = unitQueryService;
        this.dialogService = dialogService;
        this.boxService = boxService;
        this.element = element;
    }

    @computedFrom('model')
    get unit() {
        if (this.model) {
            var id;
            if (NumberHelper.isNumber(this.model)) {
                id = Number(this.model);
            } else {
                id = this.model.unitId || this.model.id || null;
            }
            if (this.model.editorKey) {
                this.editorKey = this.model.editorKey;
            }
            this.unitId = id;
            return id;

        }
        this.unitId = null;
        return null;
    }

    @computedFrom('tooltip', 'model', 'model.name', 'model.class', 'model.role', 'model.type', 'unit')
    get name() {
        if (this.tooltip && this.model) {
            if (this.model.name) {
                return this.model.name;
            } else if (this.model.class + this.model.role + this.model.type) {
                var c = NumberHelper.splitEnum(this.model.class, UnitClass).map(x => StringHelper.prettifyEnum(x.name));
                var r = NumberHelper.splitEnum(this.model.role, UnitRole).map(x => StringHelper.prettifyEnum(x.name));
                var t = NumberHelper.splitEnum(this.model.type, UnitType).map(x => x.name);

                var name = '';
                if (t.length > 0) {
                    name += ' ' + t.join(' or ');
                }
                if (c.length > 0) {
                    name += ' ' + c.join('/');
                }
                if (r.length > 0) {
                    name += ' ' + r.join(' and ');
                } else {
                    name += ' Unit';
                }
                return name;
            }
        }
        return '';
    }

    unitIdChanged(newValue, oldValue) {
        if (newValue != oldValue) {
            if (newValue) {
                this.unitQueryService.get(newValue).then(x => {
                    // prevents a promise loop
                    setTimeout(() => {
                        this.model = x;
                    }, 0);
                });
            } else if (!this.model || this.model.id === oldValue) {
                this.model = null;
            }
        }
    }

    @computedFrom('boxService.currentBox', 'box')
    get assignedBox() {
        return this.box || this.boxService.currentBox;
    }

    @computedFrom('model')
    get generic() {
        return this.model ? (this.model.role !== undefined) : false;
    }

    @computedFrom('showBox', 'unitId', 'assignedBox')
    get showBoxInput() {
        return this.showBox && this.unitId && this.assignedBox;
    }

    @computedFrom('showBoxFlags', 'unitId', 'assignedBox')
    get unitBoxFlags() {
        if (this.showBoxFlags && this.unitId) {
            var boxValue = this.assignedBox.boxUnits.find(x => x.unitId === this.unitId);
            if (boxValue) {
                return boxValue.flags;
            }
        }
        return null;
    }

    @computedFrom('showBoxInput', 'assignedBox.unitIds', 'assignedBox.unitIds.length', 'unitId')
    get hasUnit() {
        return this.showBoxInput &&
            this.assignedBox.unitIds.indexOf(this.unitId) > -1;
    }

    set hasUnit(value) {
        if (this.hasUnit !== value) {
            this.boxService.toggle(this.unitId, this.box);
        }
    }

    @computedFrom('showBoxInput', 'hasUnit')
    get unitOwnerClass() {
        return (this.showBoxInput && !this.hasUnit) ? 'no-own' : '';
    }

    @computedFrom('hasUnit', 'assignedBox', 'assignedBox.name')
    get boxTitle() {
        var boxName = this.assignedBox ? this.assignedBox.name : 'uhhh';
        return (this.hasUnit ? 'In' : 'Not in') + ' box "' + boxName + '"';
    }

    @computedFrom('icon', 'unit', 'generic', 'editable')
    get iconClass() {
        var icon = '';
        if (!this.icon) {
            icon = 'fa-' + ((this.unit || this.generic) ? 'user' : (this.editable ? 'user-plus' : 'user-o'));
        } else {
            icon = this.icon;
        }
        return 'fa fa-fw fa-2x ' + icon;
    }

    @computedFrom('info', 'editable', 'unit')
    get link() {
        return (this.info && !this.editable && this.unit)
            ? ('http://optc-db.github.io/characters/#/view/' + this.unit)
            : null;
    }

    @computedFrom('generic', 'model.type')
    get genericTypes() {
        if (this.generic) {
            return NumberHelper.splitEnum(this.model.type, UnitType);
        }
        return [];
    }

    @computedFrom('generic', 'model.class')
    get genericClasses() {
        if (this.generic) {
            return NumberHelper.splitEnum(this.model.class, UnitClass, true);
        }
        return [];
    }

    @computedFrom('generic', 'model.role')
    get genericRoles() {
        if (this.generic) {
            return NumberHelper.splitEnum(this.model.role, UnitRole, true);
        }
        return [];
    }

    @computedFrom('genericRoles')
    get countName() {
        return 'generic-' + this.genericRoles.length;
    }

    @computedFrom('unit', 'generic')
    get backgroundStyle() {
        if (this.unit || this.generic) {
            var style = 'background-image: url(\'';
            if (this.unit) {
                style += UnitQueryService.getIcon(this.unit);
            } else if (this.generic) {
                style += '/content/empty.png';
            }
            style += '\'), url(\'https://onepiece-treasurecruise.com/wp-content/themes/onepiece-treasurecruise/images/noimage.png\')';
            return style;
        }
        return '';
    }

    clicked() {
        if (this.editable) {
            var model = this.model;
            this.dialogService.open({
                viewModel: UnitPicker,
                model: <UnitPickerParams>{ model: model, allowGenerics: this.allowGenerics },
                lock: true
            }).whenClosed(result => {
                if (!result.wasCancelled) {
                    var oldModel = model;
                    this.model = result.output;
                    var bubble = new CustomEvent('selected', {
                        detail: { newValue: result.output, oldValue: oldModel, editorKey: this.editorKey, viewModel: this },
                        bubbles: true
                    });
                    this.element.dispatchEvent(bubble);
                }
            });
        } else {
            var bubble = new CustomEvent('selected', {
                detail: { newValue: this.model, editorKey: this.editorKey, viewModel: this },
                bubbles: true
            });
            this.element.dispatchEvent(bubble);
        }
    }

}