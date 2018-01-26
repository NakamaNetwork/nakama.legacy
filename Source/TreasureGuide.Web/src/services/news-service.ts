import { AlertService } from './alert-service';
import { autoinject } from 'aurelia-framework';

@autoinject
export class NewsService {
    static seenKey: string = 'LatestNewsSeen';
    static cap: number = 5;

    static news: NewsEntry[] = [
        {
            header: '01/26/18',
            lines: [
                'Users can now bookmark teams.',
                'Added new "generic unit" feature for teams, allowing users to suggest a class/type/role instead of having to pick a unit.',
                'Added unit search parameters to teams. These will also apply to generic units.',
                'Made name searching "fuzzy" to make things easier to find.',
                'Leader searching will now extend to friend captain as well. This can be disabled.',
                'Updated search interfaces to use game icons instead of text.',
                'Removed info button from unit portraits. Clicking the portrait will now go to their DB page instead.',
                'Updated valdation rules for teams.',
                'Removed team search caching. This may be re-added in the future.',
                'Fixed an issue where importing from the calculator could put units in the wrong slots.',
                'Fixed an issue where users could not save a new team as a draft.',
                'Fixed an issue where users could not view their drafts.',
                'Fixed an issue where alerts might not be dismissed automatically.',
                'Strings will now be trimmed when submitted to the server.',
                'Added "Beta Tester" role and some features that only appear to people in it. ;)',
            ]
        },
        {
            header: '01/22/18',
            lines: [
                'Added sorting features to Stage and Team searches.',
                'Updated search recalling logic to be a little more intuitive.'
            ]
        },
        {
            header: '01/21/18',
            lines: [
                'Added unit aliases from OPTC-db to make searching easier. (i.e. you can now find Golden Pound Usopp by searching "GPU")',
                'Synced units and ships with the latest data from OPTC-db.',
                'Updated search field names to be a little more user-friendly.',
                '"Name" fields will now be auto-focused when opening search panels.',
                'Your last search will be recalled when opening search panels.',
                'Users will automatically upvote their own team after submitting it.',
                'Added markdown formatting guide.',
                'Increased team guide character limit to 40,000.',
                'Increased team credit character limit to 2,000.',
                'The latest news will now be shown when visiting the site.'
            ]
        }, {
            header: '01/20/18 - Update #2',
            lines: [
                'Updated nakama.network to redirect to www.nakama.network to prevent login issues.',
                'Updated nakama.network to redirect to www.nakama.network to prevent login issues.',
            ]
        }, {
            header: '01/20/18',
            lines: [
                'Added "stages" section so users can easily find teams for each stage.',
            ]
        }, {
            header: '01/19/18 - Hotfix #1',
            lines: [
                'Fixed privacy policy on registration page linking to ToS',
                'Reduced minimum username length to 5 characters.',
                'Updated forms throughout the site to have better flow control with the enter key.',
                'Updated a few checkboxes to have larger cursor targets.'
            ]
        }, {
            header: '01/19/18',
            lines: [
                'Added links to OPTC-db for unit details.',
                'Added "New" and "Trending" team lists on front page.',
                'Moved News and About to their own pages.',
                'Updated default page sizes.',
                'Fixed an issue where rapid submitting would not be throttled properly.',
            ]
        },
        {
            header: '01/17/18',
            lines: [
                'Updated Alert, Home Page, Socket, and Pagination UI.',
                'Added better login and logout flow.',
                'Added heartbeat to ensure the user has not lost their login session.',
            ]
        }, {
            header: '01/16/18',
            lines: [
                'Added support for sockets on teams.',
                'Expanded "Free to Play" team search capabilities.',
                'Fixed Captain search to only apply to the Captain slot instead of Friend Captain and Captain.',
                'Added team flags to the team search views.',
            ]
        }, {
            header: '01/13/18',
            lines: [
                "Added a 'draft' option for teams, allowing you to save a team without making it public.",
            ]
        }, {
            header: '01/11/18',
            lines: [
                'Added support for markdown in guides and credits.',
            ]
        }, {
            header: '01/10/18',
            lines: [
                'Added better error logging for server-side issues.',
                'Fixed an issue with search model caching.',
                'Fixed an issue where unauthorized users could try and submit teams.',
                'Fixed some issues with querying on different browsers.',
                'Added calculator export button on Team Detail page.',
                'Contributors can now add videos to teams. Videos uploaded by the team owner will be emphasized. Videos uploaded by other users will be smaller.',
                'Added Reddit, Discord, and Twitch login integration.',
            ]
        }, {
            header: '01/09/18',
            lines: [
                'First release candidate has been launched!',
            ]
        }
    ];

    private alertService: AlertService;

    constructor(alertService: AlertService) {
        this.alertService = alertService;
    }

    show() {
        if (!this.hasSeen()) {
            var news = NewsService.news[0];
            var message = '<small><h6>News: ' + news.header + '</h6>';
            message += '<ul>';
            var length = Math.min(news.lines.length, NewsService.cap);
            for (var i = 0; i < length; i++) {
                message += '<li>' + news.lines[i] + '</li>';
            }
            if (news.lines.length > NewsService.cap) {
                message += '<li><em><a href="/#/news">Read More...</a></em></li>';
            }
            message += '</ul></small>';
            this.alertService.news(message);
            this.setSeen();
        }
    }

    setSeen() {
        localStorage.setItem(NewsService.seenKey, NewsService.news[0].header);
    }

    hasSeen() {
        var latest = localStorage[NewsService.seenKey];
        return latest === NewsService.news[0].header;
    }
}

export class NewsEntry {
    header: string;
    lines: string[];
}