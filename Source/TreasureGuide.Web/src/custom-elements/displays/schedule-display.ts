import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';

@autoinject
@customElement('schedule-display')
export class ScheduleDisplay {
    @bindable schedule: any[] = [];
    @bindable upcoming: boolean;

    @computedFrom('upcoming')
    get header() {
        return this.upcoming ? 'Upcoming' : 'Live';
    }

    @computedFrom('upcoming')
    get unitSize() {
        return this.upcoming ? 'tiny' : 'small';
    }

    @computedFrom('upcoming')
    get scheduleClass() {
        return this.upcoming ? 'upcoming' : 'live';
    }
}