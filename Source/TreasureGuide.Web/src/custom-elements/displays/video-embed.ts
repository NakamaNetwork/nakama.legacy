import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { PLATFORM } from 'aurelia-pal';

@customElement('video-embed')
export class VideoEmbed {
    iframe: HTMLElement;
    @bindable videoId: string;

    resizeTimer = null;
    resizeEventHandler = () => this.resized();

    attached() {
        this.resized();

        PLATFORM.global.addEventListener("resize", this.resizeEventHandler);
    }

    detached() {
        PLATFORM.global.removeEventListener("resize", this.resizeEventHandler);
    }

    resized() {
        clearTimeout(this.resizeTimer);

        this.resizeTimer = setTimeout(() => {
            if (this.iframe) {
                var width = this.iframe.clientWidth;
                (<any>this.iframe).style.height = (width * 0.5625) + 'px';
            }
        }, 150);
    }

    @computedFrom('videoId')
    get embedUrl() {
        if (this.videoId) {
            return 'https://www.youtube-nocookie.com/embed/' + this.videoId + '?rel=0';
        }
        return '';
    }
}