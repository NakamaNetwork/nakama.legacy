import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { TeamCommentQueryService, TeamCommentSearchModel } from '../../services/query/team-comment-query-service';
import { ITeamCommentStubModel, ITeamDetailModel, SearchConstants } from '../../models/imported';
import { CommentDialog } from './comment-dialog';
import { AlertService } from '../../services/alert-service';
import { AlertDialog, AlertDialogViewModel } from '../dialogs/alert-dialog';
import { BindingSignaler } from 'aurelia-templating-resources';

@autoinject
@customElement('comment-collection')
export class CommentCollection {
    private teamCommentService: TeamCommentQueryService;
    private alertService: AlertService;
    private dialogService: DialogService;
    private bindingSignaler: BindingSignaler;

    public loading: boolean;

    @bindable
    private comments: ITeamCommentStubModel[];
    @bindable
    public team: ITeamDetailModel;

    constructor(teamCommentService: TeamCommentQueryService, alertService: AlertService, dialogService: DialogService, signaler: BindingSignaler) {
        this.teamCommentService = teamCommentService;
        this.alertService = alertService;
        this.dialogService = dialogService;
        this.bindingSignaler = signaler;
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
                var newComment = !model;
                var saved = { id: id, teamId: teamId, parentId: parentId, text: result.output };
                this.teamCommentService.save(saved).then(result => {
                    this.alertService.success('Thank you! Your comment has been submitted.');
                    if (newComment && parentId) {
                        var parent = this.comments.find(x => x.id == parentId);
                        if (parent) {
                            this.loadMore(parent);
                        }
                    } else {
                        model.text = saved.text;
                    }
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
                        model.reported = false;
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
                    var index = this.comments.indexOf(model);
                    if (index >= 0) {
                        this.comments.splice(index, 1);
                    }
                }
            });
        }
    };

    loadMore(model) {
        this.teamCommentService.loadMore(model.id, model.children.length).then(result => {
            result.forEach(x => {
                var existing = model.children.find(y => y.id == x.id);
                if (!existing) {
                    model.children.push(x);
                }
            });
            this.bindingSignaler.signal('update-loads');
        });
    }

    needMore(comment) {
        return comment.childCount > comment.children.length;
    }
}