export class VideoParser {
    static YoutubeRegex: RegExp = /(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|[a-zA-Z0-9_\-]+\?v=)([^#\&\?\n<>\'\"]*)/i;

    static parse(link: string) {
        if (link) {
            var matches = link.match(VideoParser.YoutubeRegex);
            var last = matches[matches.length - 1];
            return last;
        }
        return '';
    }
}