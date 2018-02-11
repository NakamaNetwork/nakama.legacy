import { autoinject, computedFrom } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { VideoParser } from '../../tools/video-parser';
import {AlertService} from '../../services/alert-service';

@autoinject
export class VideoPicker {
    private controller: DialogController;
    private validController: ValidationController;
    private alertService: AlertService;

    model: VideoDialogViewModel;

    constructor(controller: DialogController, validFactory: ValidationControllerFactory, alertService: AlertService) {
        this.controller = controller;
        this.validController = validFactory.createForCurrentScope();
        this.validController.addRenderer(new BeauterValidationFormRenderer());
        this.model = new VideoDialogViewModel();
        this.alertService = alertService;
    }

    @computedFrom('model', 'model.link')
    get videoId() {
        return VideoParser.parse(this.model.link);
    }

    okay() {
        this.validController.validate().then(x => {
            if (x.valid) {
                this.controller.ok(this.videoId);
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    this.alertService.danger(y.message);
                });
            }
        });
    };

    cancel() {
        this.controller.cancel();
    };
}

export class VideoDialogViewModel {
    link: string;
}

ValidationRules
    .ensure((x: VideoDialogViewModel) => x.link)
    .required()
    .matches(VideoParser.YoutubeRegex)
    .withMessage('Please input a valid YouTube link.')
    .on(VideoDialogViewModel);