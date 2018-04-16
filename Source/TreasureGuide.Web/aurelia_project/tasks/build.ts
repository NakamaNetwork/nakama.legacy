import * as gulp from 'gulp';
import cleanScripts from './clean-scripts';
import copyScripts from './copy-scripts';
import transpile from './transpile';
import processMarkup from './process-markup';
import processCSS from './process-css';
import copyFiles from './copy-files';
import { build } from 'aurelia-cli';
import * as project from '../aurelia.json';
import { CLIOptions } from 'aurelia-cli';

let buildTask = gulp.series(
    readProjectConfiguration,
    cleanScripts,
    gulp.parallel(
        transpile,
        processMarkup,
        processCSS,
        copyFiles,
    ),
    copyScripts,
    writeBundles
);

let timer;

let queueBuild = () => {
    if (timer) {
        clearTimeout(timer);
    }
    timer = setTimeout(() => {
        buildTask();
    }, 1000);
}

function readProjectConfiguration() {
    return build.src(project);
}

function writeBundles() {
    return build.dest();
}

function onChange(path) {
    console.log(`File Changed: ${path}`);
}

let watch = function () {
    gulp.watch(project.transpiler.source, queueBuild).on('change', onChange);
    gulp.watch(project.markupProcessor.source, queueBuild).on('change', onChange);
    gulp.watch(project.cssProcessor.source, queueBuild).on('change', onChange)
}

let task;

if (CLIOptions.hasFlag('watch')) {
    task = gulp.series(
        buildTask,
        watch
    );
} else {
    task = buildTask;
}

export default task;