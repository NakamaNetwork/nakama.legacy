import { bindable, computedFrom, customElement } from 'aurelia-framework';

@customElement('number-dropdown')
export class NumberDropdown {
    @bindable id: string;
    @bindable value: number;
    @bindable nullable: boolean;
    @bindable min: number = 0;
    @bindable max: number = 100;

    @computedFrom('min', 'max', 'nullable')
    get options() {
        var values = [];
        if (this.nullable) {
            values.push({ value: null, text: ' - - - ' });
        }
        for (var i = this.min; i <= this.max; i++) {
            values.push({ value: i, text: i });
        }
        return values;
    }
}