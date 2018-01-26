import { bindable, computedFrom, customElement } from "aurelia-framework";
import { ITeamEditorModel } from '../models/imported';

@customElement('team-slot')
export class TeamSlot {
    @bindable team: ITeamEditorModel;
    @bindable editable: boolean;
    @bindable index: number;
    private lastUpdated: Date;

    static editorCounter: number = 0;

    @computedFrom('team', 'team.teamUnits.length', 'team.teamGenericSlots.length', 'index')
    get units() {
        var units = this.team.teamUnits.filter(x => x.position === this.index);
        var generics = this.team.teamGenericSlots.filter(x => x.position === this.index);
        var all = <any>units.concat(<any>generics);
        all.forEach((x: any) => x.editorKey = x.editorKey || ++TeamSlot.editorCounter);
        return all;
    }

    @computedFrom('units.length')
    get main() {
        return this.units.find(x => !x.sub);
    }

    @computedFrom('units.length')
    get subs() {
        return this.units.filter(x => x.sub);
    }

    @computedFrom('main', 'edtiable')
    get hasMain() {
        return this.main;
    }

    @computedFrom('hasMain', 'editable', 'subs')
    get canAddSubs() {
        return this.editable && this.hasMain && this.subs.length < 4;
    }

    unitAdded(event: CustomEvent, sub: boolean, newItem: boolean = false) {
        var newValue = event.detail.newValue;
        var oldValue = event.detail.oldValue;

        if (!newValue) {
            if (oldValue) {
                this.team.teamUnits = this.team.teamUnits.filter(x => (<any>x).editorKey !== oldValue.editorKey);
                this.team.teamGenericSlots = this.team.teamGenericSlots.filter(x => (<any>x).editorKey !== oldValue.editorKey);
                if (this.units.length > 0 && this.units.every(x => x.sub)) {
                    this.units[0].sub = false;
                }
            }
        } else {
            newValue.position = this.index;
            newValue.sub = sub;
            if (newValue.id) {
                newValue.unitId = newValue.id;
                newValue.id = undefined;
                if (oldValue && oldValue.unitId) {
                    newValue = Object.assign(oldValue, newValue);
                } else {
                    if (oldValue) {
                        this.team.teamGenericSlots = this.team.teamGenericSlots.filter(x => (<any>x).editorKey !== oldValue.editorKey);
                    }
                    newValue.editorKey = ++TeamSlot.editorCounter;
                    this.team.teamUnits.push(newValue);
                }
            } else {
                if (!oldValue || oldValue.unitId) {
                    if (oldValue) {
                        this.team.teamUnits = this.team.teamUnits.filter(x => (<any>x).editorKey !== oldValue.editorKey);
                    }
                    newValue.editorKey = ++TeamSlot.editorCounter;
                    this.team.teamGenericSlots.push(newValue);
                } else {
                    newValue = Object.assign(oldValue, newValue);
                }
            }
        }
        if (newItem) {
            event.detail.viewModel.model = undefined;
        } else {
            event.detail.viewModel.model = newValue;
        }
        this.lastUpdated = new Date();
    }
}