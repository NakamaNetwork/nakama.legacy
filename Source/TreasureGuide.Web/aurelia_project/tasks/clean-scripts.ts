import * as gulp from 'gulp';
import * as clean from 'gulp-clean';
import * as project from '../aurelia.json';

export default function cleanScripts() {
    return gulp.src(project.platform.output)
        .pipe(clean());
}
