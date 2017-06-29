import { autoinject, bindable } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { AccountQueryService } from '../../services/query/account-query-service';
import { MdToastService, MdInputUpdateService, MaterializeFormValidationRenderer } from 'aurelia-materialize-bridge';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';

@autoinject
export class RegisterPage {
    private accountQueryService: AccountQueryService;
    private toast: MdToastService;
    private inputUpdate: MdInputUpdateService;
    private router: Router;
    public controller: ValidationController;
    public providers = [];

    title = 'Register';

    public loginProvider: string;
    public returnUrl: string;

    @bindable
    public userName: string;

    @bindable
    public emailAddress: string;

    constructor(accountQueryService: AccountQueryService, router: Router, toast: MdToastService, inputUpdate: MdInputUpdateService, validFactory: ValidationControllerFactory) {
        this.accountQueryService = accountQueryService;
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new MaterializeFormValidationRenderer());

        this.toast = toast;
        this.inputUpdate = inputUpdate;
        this.router = router;
    }

    activate(params) {
        if (!params || !params.loginProvider) {
            this.router.navigateToRoute('home');
        }

        this.userName = params.userName;
        this.emailAddress = params.emailAddress;
        this.loginProvider = params.loginProvider;
        this.returnUrl = params.returnUrl;

        this.inputUpdate.update();
        this.controller.validate();
    }

    submit() {
        this.controller.validate().then(x => {
            if (x.valid) {
                this.accountQueryService.register(this.userName, this.emailAddress).then(results => {
                    this.toast.show('Welcome to NakamaDB!', 5000);
                    this.router.navigateToRoute('home');
                }).catch(results => {
                    console.error(results);
                });
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    this.toast.show(y.message, 5000);
                });
            }
        });
    }
}

export var UserNameRegex: RegExp = /[^A-Za-z0-9\-\._\@\+ ]/;

ValidationRules
    .ensure((x: RegisterPage) => x.emailAddress)
    .required()
    .email()
    .maxLength(256)
    .ensure((x: RegisterPage) => x.userName)
    .required()
    .satisfies((x: string) => !UserNameRegex.test(x))
    .withMessage('User Name may contain alphanumeric characters only.')
    .maxLength(256)
    .on(RegisterPage);