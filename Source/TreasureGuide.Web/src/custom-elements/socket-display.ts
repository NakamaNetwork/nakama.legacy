import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { ITeamSocketModel } from '../models/imported';
import { SocketType } from '../models/imported';
import { StringHelper } from '../tools/string-helper';
import { SocketConstants } from '../constants/socket-constants';

@customElement('socket-display')
@autoinject
export class SocketDisplay {
    @bindable sockets: ITeamSocketModel[] = [];
    @bindable editable = false;

    @computedFrom('sockets')
    get socketEntries() {
        var socketEntries = [];
        for (var item in SocketType) {
            var id = Number(item);
            if (!isNaN(id) && id > 0) {
                var match = this.sockets.find(x => x.socketType === id);
                if (!match) {
                    match = <ITeamSocketModel>{
                        socketType: id,
                        level: 0
                    };
                    this.sockets.push(match);
                }
                var entry = {
                    id: id,
                    name: StringHelper.prettifyEnum(SocketType[item]),
                    match: match,
                    max: SocketConstants.maxLevels[id],
                    element: 'socket-' + SocketType[item],
                    image: '/content/sockets/' + id + '.png'
                };
                socketEntries.push(entry);
            }
        }
        socketEntries = socketEntries.sort((a, b) => a.id - b.id);
        return socketEntries;
    }
}