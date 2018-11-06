import { autoinject, bindable, customElement } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { TeamCommentQueryService, TeamCommentSearchModel } from '../../services/query/team-comment-query-service';
import { ITeamCommentStubModel, ITeamDetailModel, SearchConstants } from '../../models/imported';
import { CommentDialog } from './comment-dialog';
import { AlertService } from '../../services/alert-service';
import { AlertDialog } from '../dialogs/alert-dialog';
import { AlertDialogViewModel } from '../dialogs/alert-dialog';

@autoinject
@customElement('comment-collection')
export class CommentCollection {
    private teamCommentService: TeamCommentQueryService;
    private alertService: AlertService;
    private dialogService: DialogService;

    public loading: boolean;
    @bindable
    private comments: ITeamCommentStubModel[];
    @bindable
    public team: ITeamDetailModel;
    
    constructor(teamCommentService: TeamCommentQueryService, alertService: AlertService, dialogService: DialogService) {
        this.teamCommentService = teamCommentService;
        this.alertService = alertService;
        this.dialogService = dialogService;
    }

    edit(model, parentId) {
        var id = null;
        var teamId = null;
        if (model) {
            id = model.id;
            teamId = model.teamId;
            parentId = model.parentId;
        }
        this.dialogService.open({ viewModel: CommentDialog, model: model, lock: true }).whenClosed(result => {
            if (!result.wasCancelled) {
                if (!teamId) {
                    teamId = this.team.id;
                }
                this.teamCommentService.save({ id: id, teamId: teamId, parentId: parentId, text: <string>result.output }).then(result => {
                    this.alertService.success('Thank you! Your comment has been submitted.');
                }).catch(response => this.alertService.reportError(response));
            }
        });
    }

    acknowledge(model) {
        if (model) {
            this.dialogService.open({
                viewModel: AlertDialog,
                model: <AlertDialogViewModel>{
                    message: 'Are you sure you want to clear the reports on this comment?',
                    cancelable: true
                },
                lock: true
            }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.teamCommentService.acknowledge({ teamCommentId: model.id }).then(result => {
                        this.alertService.success('The reports have been cleared.');
                    });
                }
            });
        }
    };

    delete(model) {
        if (model) {
            this.dialogService.open({
                viewModel: AlertDialog,
                model: <AlertDialogViewModel>{
                    message: 'Are you sure you want to delete this comment?',
                    cancelable: true
                },
                lock: true
            }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.teamCommentService.delete(model.id).then(result => {
                        this.alertService.success('Your comment has been deleted.');
                    });
                }
            });
        }
    };
}