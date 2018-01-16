export class ErrorPage {
    title = 'Error';
    message = 'Sorry! An error has occurred. Please try again.';

    activate(params) {
        if (params.error) {
            this.message = params.error;
        }
    }
}