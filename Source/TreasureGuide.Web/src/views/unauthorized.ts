﻿export class ErrorPage {
    title = 'Unauthorized';
    message = 'You do not have permission to access this stuff.';

    activate(params) {
        if (params.error) {
            this.message = params.error;
        }
    }
}