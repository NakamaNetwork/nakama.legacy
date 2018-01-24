import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitQueryService } from '../services/query/unit-query-service';
import { DialogService } from 'aurelia-dialog';
import { NumberHelper } from '../tools/number-helper';
import { UnitPicker } from './dialogs/unit-picker';
import { UnitPickerParams } from './dialogs/unit-picker';
import { IUnitEditorModel } from '../models/imported';

@autoinject
@customElement('unit-display')
export class UnitDisplay {
    private element: Element;
    private unitQueryService: UnitQueryService;
    private dialogService: DialogService;

    @bindable unitId: number;
    @bindable model: any;
    @bindable editable: boolean;
    @bindable allowGenerics: boolean;
    @bindable info: boolean;

    constructor(unitQueryService: UnitQueryService, dialogService: DialogService, element: Element) {
        this.unitQueryService = unitQueryService;
        this.element = element;
        this.dialogService = dialogService;
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
            this.unitId = id;
            return id;

        }
        return null;
    }

    unitIdChanged(newValue, oldValue) {
        if (this.unit != newValue) {
            if (newValue) {
                this.model = <IUnitEditorModel>{ id: newValue };
            } else {
                this.model = null;
            }
        }
    }

    @computedFrom('model')
    get generic() {
        return this.model ? (this.model.roles !== undefined) : false;
    }

    @computedFrom('unit', 'generic', 'editable')
    get iconClass() {
        return 'fa fa-fw fa-2x fa-' + ((this.unit || this.generic) ? 'user' : (this.editable ? 'user-plus' : 'user-o'));
    }

    @computedFrom('info', 'editable', 'unit')
    get link() {
        return (this.info && !this.editable && this.unit)
            ? ('http://optc-db.github.io/characters/#/view/' + this.unit)
            : null;
    }

    @computedFrom('generic')
    get genericTypes() {
        return [];
    }

    @computedFrom('generic')
    get genericClasses() {
        return [];
    }

    @computedFrom('generic')
    get genericRoles() {
        return [];
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
            this.dialogService.open({ viewModel: UnitPicker, model: <UnitPickerParams>{ model: model, allowGenerics: this.allowGenerics }, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    var oldModel = model;
                    this.model = result.output;
                    var event = new CustomEvent('selected', {
                        detail: { newValue: result.output, oldValue: oldModel, viewModel: this },
                        bubbles: true
                    });
                    this.element.dispatchEvent(event);
                }
            });
        }
    }

}