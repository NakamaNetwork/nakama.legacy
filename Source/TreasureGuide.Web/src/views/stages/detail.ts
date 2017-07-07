import { autoinject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { StageQueryService } from '../../services/query/stage-query-service';

@autoinject
export class StageDetailPage {
    private stageQueryService: StageQueryService;
    private router: Router;

    title = 'Stage Details';
    stage;

    constructor(stageQueryService: StageQueryService, router: Router) {
        this.stageQueryService = stageQueryService;
        this.router = router;
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.stageQueryService.editor(id).then(result => {
                this.stage = result;
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested stage could not be found.' });
            });
        } else {
            this.router.navigateToRoute('error', { error: 'The requested stage could not be found.' });
        }
    }
}