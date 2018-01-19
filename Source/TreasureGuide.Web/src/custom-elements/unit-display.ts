import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitQueryService } from '../services/query/unit-query-service';
import { DialogService } from 'aurelia-dialog';
import { UnitPicker } from './dialogs/unit-picker';

@autoinject
@customElement('unit-display')
export class UnitDisplay {
    private element: Element;
    private unitQueryService: UnitQueryService;
    private dialogService: DialogService;

    @bindable unitId = 0;
    @bindable name: string;
    @bindable editable = false;
    @bindable clickable = false;
    @bindable info = true;

    constructor(unitQueryService: UnitQueryService, dialogService: DialogService, element: Element) {
        this.unitQueryService = unitQueryService;
        this.element = element;
        this.dialogService = dialogService;
    }

    @computedFrom('unitId')
    get imageUrl() {
        return UnitQueryService.getIcon(this.unitId);
    }

    @computedFrom('unitId', 'editable')
    get iconClass() {
        return 'fa fa-fw fa-2x fa-' + (this.unitId ? 'user' : (this.editable ? 'user-plus' : 'user-o'));
    }

    @computedFrom('editable', 'clickable')
    get buttonClass() {
        return 'unit-button ' + (this.editable || this.clickable ? '' : '_unclickable');
    }

    @computedFrom('unitId', 'imageUrl')
    get backgroundStyle() {
        if (this.unitId) {
            return 'background-image: url(\'' + this.imageUrl + '\'), url(\'https://onepiece-treasurecruise.com/wp-content/themes/onepiece-treasurecruise/images/noimage.png\')';
        }
        return '';
    }

    @computedFrom('unitId')
    get link() {
        return 'http://optc-db.github.io/characters/#/view/' + this.unitId;
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