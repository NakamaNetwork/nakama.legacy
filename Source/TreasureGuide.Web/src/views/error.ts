export class ErrorPage {
    title = 'Error';
    message = 'An error has occurred.';

    activate(params) {
        if (params.error) {
            this.message = params.error;
        }
    }
}