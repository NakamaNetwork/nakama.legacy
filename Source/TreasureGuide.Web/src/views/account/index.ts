import { autoinject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { ProfileQueryService } from '../../services/query/profile-query-service';
import { IProfileDetailModel } from '../../models/imported';

@autoinject
export class ProfileDetailPage {
    private profileQueryService: ProfileQueryService;
    private router: Router;

    profile: IProfileDetailModel;

    constructor(profileQueryService: ProfileQueryService, router: Router) {
        this.profileQueryService = profileQueryService;
        this.router = router;
    }

    activate(params) {
        this.profileQueryService.detail(params.id).then(result => {
            this.profile = result;
        }).catch(error => {
            this.router.navigateToRoute('error', { error: 'The requested account could not be found.' });
        });
    }
}