import * as gulp from 'gulp';
import * as sourcemaps from 'gulp-sourcemaps';
import * as sass from 'gulp-sass';
import * as cleancss from 'gulp-clean-css';
import * as project from '../aurelia.json';
import { build } from 'aurelia-cli';

export default function processCSS() {
    return gulp.src(project.cssProcessor.source)
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(cleancss({
            compatibility: 'ie8',
            level: 2
        }))
        .pipe(gulp.dest(project.platform.baseDir));
};
