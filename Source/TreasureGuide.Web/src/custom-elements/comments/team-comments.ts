import { autoinject, bindable, customElement } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { TeamCommentQueryService, TeamCommentSearchModel } from '../../services/query/team-comment-query-service';
import { ITeamCommentStubModel, ITeamDetailModel, SearchConstants } from '../../models/imported';
import { PLATFORM } from 'aurelia-pal';
import { CommentDialog } from './comment-dialog';
import { AlertService } from '../../services/alert-service';

@autoinject
@customElement('team-comments')
export class TeamComments {
    private teamCommentService: TeamCommentQueryService;
    private element: HTMLElement;
    private bindingEngine: BindingEngine;
    private alertService: AlertService;
    private dialogService: DialogService;

    @bindable
    public team: ITeamDetailModel;
    public seen: boolean;
    public loading: boolean;
    private comments: ITeamCommentStubModel[];

    public searchModel: TeamCommentSearchModel;

    constructor(teamCommentService: TeamCommentQueryService, element: Element,
        bindingEngine: BindingEngine, alertService: AlertService, dialogService: DialogService) {
        this.teamCommentService = teamCommentService;
        this.element = <HTMLElement>element;
        this.bindingEngine = bindingEngine;
        this.alertService = alertService;
        this.dialogService = dialogService;
    }

    refreshTimer = null;
    refreshEventHandler = () => this.refreshed();

    attached() {
        this.searchModel = new TeamCommentSearchModel();
        this.searchModel.teamId = this.team.id;
        this.refreshed();

        PLATFORM.global.addEventListener("scroll", this.refreshEventHandler);
        PLATFORM.global.addEventListener("resize", this.refreshEventHandler);
    }

    detached() {
        this.disableLoading();
    }

    refreshed() {
        clearTimeout(this.refreshTimer);

        this.refreshTimer = setTimeout(() => {
            if (this.isInViewport()) {
                this.loadComments();
            }
        }, 150);
    }

    loadComments() {
        this.seen = true;
        this.loading = true;
        this.disableLoading();
        this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    search(payload) {
        if (this.teamCommentService) {
            this.loading = true;
            this.teamCommentService.search(payload).then(x => {
                this.comments = x.results;
                this.searchModel.totalResults = x.totalResults;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }

    disableLoading() {
        PLATFORM.global.removeEventListener("scroll", this.refreshEventHandler);
        PLATFORM.global.removeEventListener("resize", this.refreshEventHandler);
    }

    isInViewport(): boolean {
        var rect = this.element.getBoundingClientRect();

        return (
            rect.top >= 0 &&
            rect.top <= (window.innerHeight || document.documentElement.clientHeight)
        );
    }

    comment(model) {
        var id = null;
        if (model) {
            id = model.id;
        }
        this.dialogService.open({ viewModel: CommentDialog, model: model, lock: true }).whenClosed(result => {
            if (!result.wasCancelled) {
                this.teamCommentService.save({ id: id, teamId: this.team.id, text: <string>result.output }).then(result => {
                    this.alertService.success('Thank you! Your comment has been submitted.');
                    this.searchModel.sortBy = SearchConstants.SortDate;
                    this.searchModel.sortDesc = false;
                }).catch(response => this.alertService.reportError(response));
            }
        });
    }
}