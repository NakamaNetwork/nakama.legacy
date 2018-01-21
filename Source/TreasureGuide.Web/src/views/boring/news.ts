import { NewsEntry } from '../../services/news-service';
import { NewsService } from '../../services/news-service';
import { autoinject } from 'aurelia-framework';

@autoinject
export class NewsPage {
    private newsService: NewsService;

    news: NewsEntry[] = [];

    constructor(newsService: NewsService) {
        this.newsService = newsService;
        this.news = NewsService.news;
    }

    activate() {
        this.newsService.setSeen();
    }
}