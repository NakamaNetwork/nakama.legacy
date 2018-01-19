import { bindable, computedFrom, customElement } from 'aurelia-framework';

@customElement('friend-id-display')
export class FriendIdDisplay {
    @bindable global = false;
    @bindable friendId = '';

    @computedFrom('friendId')
    get display() {
        if (this.friendId) {
            var m = this.friendId.toString().match(/^(\d{3})(\d{3})(\d{3})$/);
            return (!m) ? null : m[1] + '-' + m[2] + '-' + m[3];
        }
        return '';
    }
}