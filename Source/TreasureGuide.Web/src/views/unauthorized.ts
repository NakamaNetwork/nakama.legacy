export class ErrorPage {
    title = 'Unauthorized';
    message = 'You do not have permission to access this page.';

    activate(params) {
        if (params.error) {
            this.message = params.error;
        }
    }
}