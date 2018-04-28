import * as gulp from 'gulp';
import * as path from 'path';
import * as project from '../aurelia.json';
import * as fs from 'fs';

export default function copyScripts(done) {
    let source = project.copyScripts.source;

    let task = gulp.src(source)
        .pipe(gulp.dest('wwwroot/scripts/static'));

    return task;
}
