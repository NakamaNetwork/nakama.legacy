import { autoinject } from 'aurelia-dependency-injection';
import { Router } from 'aurelia-router';
import { StageQueryService } from '../../services/query/stage-query-service';
import { MdToastService, MdInputUpdateService } from 'aurelia-materialize-bridge';
import { StageType } from '../../models/stage-type';

@autoinject
export class StageEditPage {
    private stageQueryService: StageQueryService;
    private toast: MdToastService;
    private inputUpdate: MdInputUpdateService;
    private router: Router;

    title = 'Create Stage';
    stage = {
        name: '',
        stamina: null,
        global: true,
        type: null,
        parentId: null
    };
    stageTypes = StageType.all;

    constructor(stageQueryService: StageQueryService, router: Router, toast: MdToastService, inputUpdate: MdInputUpdateService) {
        this.stageQueryService = stageQueryService;
        this.toast = toast;
        this.inputUpdate = inputUpdate;
        this.router = router;
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.stageQueryService.editor(id).then(result => {
                this.title = 'Edit Stage';
                this.stage = result;
                this.inputUpdate.update();
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested stage could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
    }

    submit() {
        this.stageQueryService.save(this.stage).then(results => {
            this.toast.show('Successfully saved ' + this.stage.name + ' to server!', 2000);
            this.router.navigateToRoute('stageDetails', { id: results });
        }).catch(results => {
            console.error(results);
        });
    }
}