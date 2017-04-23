import * as gulp from 'gulp';
import transpile from './transpile';
import prepareMaterialize from './prepare-materialize';
import processMarkup from './process-markup';
import processCSS from './process-css';
import copyFiles from './copy-files';
import { build } from 'aurelia-cli';
import * as project from '../aurelia.json';

export default gulp.series(
    readProjectConfiguration,
    gulp.parallel(
        transpile,
        prepareMaterialize,
        processMarkup,
        processCSS,
        copyFiles
    ),
    writeBundles
);

function readProjectConfiguration() {
    return build.src(project);
}

function writeBundles() {
    return build.dest();
}
