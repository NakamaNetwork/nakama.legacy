import * as gulp from 'gulp';
import * as clean from 'gulp-clean';
import * as project from '../aurelia.json';
import * as fs from 'fs';

export default function cleanScripts(done) {
    if (fs.existsSync(project.platform.output)) {
        return gulp.src(project.platform.output)
            .pipe(clean());
    }
    done();
}
