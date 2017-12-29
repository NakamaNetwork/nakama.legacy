import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitQueryService } from '../services/query/unit-query-service';
import { DialogService } from 'aurelia-dialog';
import { UnitPicker } from './unit-picker';

@autoinject
@customElement('unit-display')
export class UnitDisplay {
    private element: Element;
    private unitQueryService: UnitQueryService;
    private dialogService: DialogService;

    @bindable unitId = 0;
    @bindable editable = false;

    constructor(unitQueryService: UnitQueryService, dialogService: DialogService, element: Element) {
        this.unitQueryService = unitQueryService;
        this.element = element;
        this.dialogService = dialogService;
    }

    @computedFrom('unitId')
    get unit() {
        return this.unitQueryService.stub(this.unitId).then(result => {
            return result;
        }).catch(error => {
            console.error(error);
            return null;
        });
    };

    @computedFrom('unitId')
    get imageUrl() {
        return this.unitQueryService.getIcon(this.unitId);
    }

    @computedFrom('unitId', 'editable')
    get iconClass() {
        return 'fa fa-fw fa-2x fa-' + (this.unitId ? 'user' : (this.editable ? 'user-plus' : 'user-o'));
    }

    @computedFrom('editable')
    get buttonClass() {
        return 'unit-button ' + (this.editable ? '' : '_unclickable');
    }

    clicked() {
        if (this.editable) {
            this.dialogService.open({ viewModel: UnitPicker, model: { unitId: this.unitId }, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.unitId = result.output;
                }
            });
        }
    }
}