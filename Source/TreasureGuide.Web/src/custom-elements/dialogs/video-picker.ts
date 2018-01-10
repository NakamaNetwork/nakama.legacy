import { autoinject, computedFrom } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';

@autoinject
export class VideoPicker {
    static YoutubeRegex: RegExp = /(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|[a-zA-Z0-9_\-]+\?v=)([^#\&\?\n<>\'\"]*)/i;
    private controller: DialogController;
    private validController: ValidationController;

    model: VideoDialogViewModel;

    constructor(controller: DialogController, validFactory: ValidationControllerFactory) {
        this.controller = controller;
        this.validController = validFactory.createForCurrentScope();
        this.validController.addRenderer(new BeauterValidationFormRenderer());
        this.model = new VideoDialogViewModel();
    }

    @computedFrom('model', 'model.link')
    get videoId() {
        if (this.model.link) {
            var matches = this.model.link.match(VideoPicker.YoutubeRegex);
            var last = matches[matches.length - 1];
            return last;
        }
        return '';
    }

    okay() {
        this.validController.validate().then(x => {
            if (x.valid) {
                this.controller.ok(this.videoId);
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
    .matches(VideoPicker.YoutubeRegex)
    .withMessage('Please input a valid YouTube link.')
    .on(VideoDialogViewModel);