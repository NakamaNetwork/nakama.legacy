/// <binding />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var shell = require('gulp-shell');

gulp.task('build-dev', shell.task(['au build --env dev']));
gulp.task('build-dev watch', shell.task(['au build --env dev --watch']));
gulp.task('build-prod', shell.task(['au build --env prod']));
gulp.task('build-stage', shell.task(['au build --env stage']));
gulp.task('run', shell.task(['au run']));
gulp.task('run watch', shell.task(['au run --watch']));
gulp.task('test', shell.task(['au test']));
gulp.task('test watch', shell.task(['au test --watch']));

console.log('--- NOTICE ---'
    + '\n'
    + 'If these tasks fail due to an error "Error: spawn au build --env dev ENOENT", go to "Tools > Options > Web Package Management > Projects and Solutions > External Web Tools" '
    + 'and move your $(PATH) entry above the $(VSINSTALLDIR) entries.'
    + '\n\n'
    + 'Visual Studio comes bundled with an old version of node which does not support the spawning of the aurelia build task.'
    + '\n\n'
    + 'See: https://blogs.msdn.microsoft.com/webdev/2015/03/19/customize-external-web-tools-in-visual-studio-2015/ for more details.'
    + '\n'
    + '--------------');