import { ValidationRenderer, RenderInstruction, ValidateResult } from 'aurelia-validation';

export class BeauterValidationFormRenderer implements ValidationRenderer {
    private static containerQuery = '.form-group';
    private static containerErrorClass = 'validation-error';
    private static messageErrorClass = 'validation-message';

    render(instruction: RenderInstruction) {
        for (let { result, elements } of instruction.unrender) {
            for (let element of elements) {
                this.remove(element, result);
            }
        }

        for (let { result, elements } of instruction.render) {
            for (let element of elements) {
                this.add(element, result);
            }
        }
    }

    add(element: Element, result: ValidateResult) {
        if (result.valid) {
            return;
        }

        const formGroup = element.closest(BeauterValidationFormRenderer.containerQuery);
        if (!formGroup) {
            return;
        }

        // add the has-error class to the enclosing form-group div
        formGroup.classList.add(BeauterValidationFormRenderer.containerErrorClass);

        // add help-block
        const message = document.createElement('span');
        message.className = BeauterValidationFormRenderer.messageErrorClass;
        message.textContent = result.message;
        message.id = `validation-message-${result.id}`;
        formGroup.appendChild(message);
    }

    remove(element: Element, result: ValidateResult) {
        if (result.valid) {
            return;
        }

        const formGroup = element.closest(BeauterValidationFormRenderer.containerQuery);
        if (!formGroup) {
            return;
        }

        // remove help-block
        const message = formGroup.querySelector(`#validation-message-${result.id}`);
        if (message) {
            formGroup.removeChild(message);

            // remove the has-error class from the enclosing form-group div
            if (formGroup.querySelectorAll(BeauterValidationFormRenderer.messageErrorClass).length === 0) {
                formGroup.classList.remove(BeauterValidationFormRenderer.containerErrorClass);
            }
        }
    }
}