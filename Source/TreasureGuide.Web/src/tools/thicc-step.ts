import { autoinject } from 'aurelia-framework';
import { NavigationInstruction } from 'aurelia-router';

@autoinject
export class ThiccStep {
    private static ContainerClass: string = 'container';

    run(routingContext: NavigationInstruction, next) {
        return ThiccStep.force(routingContext, next);
    }

    public static cleanForce(routingContext: NavigationInstruction) {
        return ThiccStep.force(routingContext, () => { });
    }

    public static force(routingContext: NavigationInstruction, next) {
        if (routingContext) {
            var thicc = routingContext.getAllInstructions().map(i => i.config['thicc']).filter(i => i);
            var container = document.getElementById(ThiccStep.ContainerClass);
            if (container) {
                if (thicc.length > 0) {
                    if (container.classList.contains(ThiccStep.ContainerClass)) {
                        container.classList.remove(ThiccStep.ContainerClass);
                    }
                } else {
                    if (!container.classList.contains(ThiccStep.ContainerClass)) {
                        container.classList.add(ThiccStep.ContainerClass);
                    }
                }
            }
        }
        return next();
    }
}