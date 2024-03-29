import { AlertService } from './alert-service';
import { autoinject } from 'aurelia-framework';

@autoinject
export class NewsService {
    static seenKey: string = 'LatestNewsSeen';
    static cap: number = 5;

    static news: NewsEntry[] = [{
        header: '04/28/22',
        lines: [
            'Added new category for Grand Voyage missions.'
        ]
    },{
        header: '04/07/21',
        lines: [
            'Added proper support for Arena stages and Rookie Missions.'
        ]
    },{
        header: '02/25/21',
        lines: [
            'Reduced the timeout on rate limiting so hopefully nobody will experience it via normal site usage.'
        ]
    },{
        header: '02/03/21',
        lines: [
            'Updated cookie configurations to allow for longer sign-in sessions.'
        ]
    }, {
        header: '01/18/21',
        lines: [
            'Populated missing unit icons.',
            'Moved all unit icons to a self-hosted CDN.']
    }, {
        header: '07/20/20',
        lines: [
            'Fixed an issue where calculator links would load support units.']
    }, {
        header: '06/02/20',
        lines: [
            'Today we\'re going dark in solidarity with our Nakama in the United States.'
        ]
    }, {
        header: '11/30/19',
        lines: [
            'Increased support for Kizuna Clash.',
            'Note I said "Kizuna" and not "Kisuna"',
        ]
    }, {
        header: '10/10/19',
        lines: [
            'Added Kizuna Clash stage support.',
        ]
    }, {
        header: '09/16/19',
        lines: [
            'Increased the size of embedded videos on teams to make it easier to navigate on mobile.',
        ]
    }, {
        header: '06/06/19',
        lines: [
            'Fixed issue in which support units would show up in team search results instead of the primary units.',
            'Fixed an issue that would cause Friend Ids with leading zeroes to display incorrectly.',
        ]
    }, {
        header: '05/20/19',
        lines: [
            'Added support for Support Units in teams.',
            'Added new "Support Maxed" flag for box units.',
            'Added "Has Support Units" search filter and team overview flag.'
        ]
    }, {
        header: '05/16/19',
        lines: [
            'Added new "Rainbowed" flag for Box Units.',
            'Unit name input field will now be highlighted when you open the unit menu so you can start typing immediately.',
            'Your own comments will now be highlighted in the comments lists.',
            'Updated notification page to make notification dismissal much smoother.',
            'Added 20 character minimum for team guides to encourage better submissions.',
            'Finally got around to removing duplicate "SPECIAL ACTIVATED" versions of ships.',
            'Adjusted session logic to hopefully cut down on unexpected logouts.'
        ]
    }, {
        header: '04/16/19',
        lines: [
            'Updated access token storage mechanism to reduce the amount of unwanted logouts.',
        ]
    }, {
        header: '03/29/19',
        lines: [
            'Updated look-and-feel of comments section to be much more mobile-friendly.',
            'Fixed a bug that prevented comment replies from being editable.',
            'Removed the forced redirect to the About page when first browsing to Nakama Network.',
            'Removed the big title box on the main page.'
        ]
    }, {
        header: '02/24/19',
        lines: [
            'Added several extra Generic Unit roles.',
            'Now allowing up to two roles to be defined per generic unit.',
            'Added additional logging in backend calls.'
        ]
    }, {
        header: '02/02/19',
        lines: [
            'Rewrote Google authentication module in preparation for Google Plus deprecation this month.'
        ]
    }, {
        header: '01/14/19',
        lines: [
            'Revised one of the weekend\'s "significantly faster" database updates as it was causing things to go significantly not faster.'
        ]
    }
        //}, {
        //    header: '01/12/19',
        //    lines: [
        //        'Happy first anniversary, Nakama Network!',
        //        'Rewrote scoring backend to make score queries significantly faster.',
        //        'Added new indexing to database to make team queries significantly faster.',
        //        'Fixed a bug where comment replies would appear at the top-level.',
        //        'Completely removed Similar Teams feature due to lack of engagement.'
        //    ]
        //}, {
        //    header: '11/25/18',
        //    lines: [
        //        'Added some error messages for failed searches instead of just "No results found."',
        //        'Reduced the size and frequency of notification queries to help stem some DB performance issues.',
        //        'Updated some very inefficient team searching queries.',
        //        'Added a cap to DB query runtime to force-terminate long-running queries.',
        //        'Fixed an issue where Notification queries could run multiple times.'
        //    ]
        //}, {
        //    header: '11/07/18',
        //    lines: [
        //        'Added separate "edited" date display on teams and team comments. This has been applied retroactively to all applicable items.',
        //        'Updated Team Summaries to show Submitted Dates instead of Edited Dates',
        //        'Fixed an issue that would prevent team searches from updating when changing the leader filter.',
        //        'Gave all search models a default sorting state.',
        //        'Removed ability to turn off search sorting.',
        //        'Added a check to prevent comments from attempting to load when none exist.',
        //        'Fixed the Twitter login button showing up in the wrong place on desktop screens.'
        //    ]
        //}, {
        //    header: '11/06/18',
        //    lines: [
        //        'Removed duplicate dual unit entries from the character database. All team and box references to these units have been automatically updated to use their correct versions.',
        //        'Added the ability to reply to top-level comments.',
        //        'Added notifications for comments, comment replies, and video posts.'
        //    ]
        //}, {
        //    header: '10/10/18',
        //    lines: [
        //        'Fixed an issue that was causing events to be removed from "featured content" too soon.'
        //    ]
        //}, {
        //    header: '09/09/18',
        //    lines: [
        //        'At long last, user can now post comments on teams!',
        //        'Updated coliseum stages to show the highest evo of the character as the icon and name.',
        //        'Users can vote on comments. Comments will be sorted by highest score by default.',
        //        'Merged featured content bars into one and reduced the date range to 5 days instead of 7.',
        //        'Removed JPN option from featured content bar as we don\'t have a source for it at the moment.',
        //        'Added a character requirement to team guides to encourage people to write something.',
        //        'Removed "similar teams" feature from team detail pages due to lack of engagement.',
        //        'If data is not showing properly, try clearing your cache, cookies, and saved website data.'
        //    ]
        //}, {
        //    header: '06/25/18',
        //    lines: [
        //        'Team Search now includes event ships by default. Added "Exclude Event Ships" option to team search instead.',
        //    ]
        //}, {
        //    header: '06/19/18 - #2',
        //    lines: [
        //        'Fixed an issue with the team submission validator that prevented teams with similar (but not duplicate) generic units from being submitted.'
        //    ]
        //}, {
        //    header: '06/19/18',
        //    lines: [
        //        'Added some new team query endpoints for use by the OPTC Wiki.'
        //    ]
        //}, {
        //    header: '06/18/18',
        //    lines: [
        //        'Fixed an issue that could cause uploaded video links to break during submission.'
        //    ]
        //}, {
        //    header: '06/11/18',
        //    lines: [
        //        'Updated the way data caching works to significantly reduce database load.',
        //        'Migrated database to a new Amazon RDS instance.',
        //        'Updated name of "Support" page.',
        //        'Added a direct link to the "Donate" page from the menu bar.'
        //    ]
        //}, {
        //    header: '05/04/18',
        //    lines: [
        //        'Added new icons for team box flags (i.e. Global, F2P, Videos)',
        //        'Fixed unit icons for Cerebral Doffy.'
        //    ]
        //}, {
        //    header: '04/27/18',
        //    lines: [
        //        'Added Zombies and Nukers to Generic Unit Roles.',
        //        'Units can now be added and removed from user boxes via Team Boxes.',
        //        'Added Markdown support for unit text colors.',
        //        'Markdown links now pop open in a new window.'
        //    ]
        //}, {
        //    header: '03/23/18',
        //    lines: [
        //        'Added a live and upcoming events bar to the home page. Powered by OPTC-Agenda!',
        //        'Added a "Submit Team" button on stage detail pages.'
        //    ]
        //}, {
        //    header: '03/22/18',
        //    lines: [
        //        'Fixed Raid Doflamingo v2 Unit Ids (you may need to re-add him to your boxes.)',
        //        'Updated locally cached data services to cut down on duplicate units.'
        //    ]
        //}, {
        //    header: '03/19/18',
        //    lines: [
        //        'Fixed search filters so users can finally see their own drafts again.',
        //        'Fixed a bug that would cause generic units to save their type incorrectly.'
        //    ]
        //}, {
        //    header: '03/04/18',
        //    lines: [
        //        'Updated the default sorting rules for units.',
        //        'Restored tooltips for unit portraits in unit picker dialogs.'
        //    ]
        //}, {
        //    header: '03/02/18',
        //    lines: [
        //        'All users can now specify suggested unit subs when editing teams.',
        //        'Added "Similar Teams" listing to team detail pages.',
        //        'Updated the "Similar Team" logic to show more relevant teams.',
        //        'Added new aliases for stages to help make search easier.',
        //        'Updated the text searching logic throughout the site.',
        //        'Exporting to OPTCdb\'s calculator will once again set all the units to max level.',
        //        'Fixed a few rendering issues with unit portraits.',
        //        'Enhanced rate limiting to help prevent overloading the site. (This should not be visible to anyone using the site normally.)'
        //    ]
        //},
        //{
        //    header: '02/28/18',
        //    lines: [
        //        'Introduced local caching for values that don\'t change frequently (i.e. units, stages, ships) which will significantly reduce the number of queries needed when browsing the site.',
        //        'New teams now must have a stage specified.',
        //        'Reduced coliseum stage entries to just an "opening stages" and "final stage" category, instead of one for every difficulty and stage.',
        //        'Fixed an issue that prevented the \'similar teams\' functionality from working.'
        //    ]
        //},
        //{
        //    header: '02/25/18',
        //    lines: [
        //        'Teams can now be marked with an Invasion specification.',
        //        'Added icons to stages.',
        //        'Updated unit sorting logic to more closely resemble that of OPTC. (Not all of them are 100% accurate still.)',
        //        'Updated order of unit classes to more closely resemble that of OPTC.',
        //        'Added a few small perks for those who donate to the development and hosting of Nakama Network. See "Support" page for more details.',
        //        'Give me all your money.'
        //    ]
        //},
        //{
        //    header: '02/19/18 - Continued',
        //    lines: [
        //        'All users have been granted an additional box slot!',
        //        'Users can now set flags on their box units to indicate maxed levels, skills, sockets, cotton candies, and more!',
        //        'Updated the way we generate Stage Ids.',
        //        'Added Aliases for Stages to make searching easier.',
        //        'Box units can now be "featured" at the top of the box to better show-off cool stuff.',
        //        'Clicking a unit portrait in the add/remove box dialog will now toggle it.',
        //        'Clicking a unit portrait in the box detail page will now open its info page on OPTC-db.',
        //        'Fixed an issue where box deletion could fail.',
        //        'Fixed an issue where creating a box would not immediately open it for the user.',
        //        'Fixed an issue where editing teams could fail.'
        //    ]
        //},
        //{
        //    header: '02/17/18',
        //    lines: [
        //        'Added sorting to unit search areas.',
        //        'Added new icons for active event ships and inactive event ships.',
        //        'Team searches will exclude teams using inactive even ships by default.',
        //        'Added option in team searches to include teams using inactive event ships.'
        //    ]
        //},
        //{
        //    header: '02/11/18 - The Box Update',
        //    lines: [
        //        'Added new "Box" feature in which users can create a "Box" to keep track of units they own and don\'t own.',
        //        'Added "Box Filter" option for Unit and Team searches that will only show teams that match your box.',
        //        'Removed "Friend Id" and "Global" fields from user profiles and moved them into boxes.',
        //        'Users can view each other\'s boxes from their profile pages (if set to public.)',
        //        'Added missing Discord icon on login screen.',
        //        'Fixed an issue where unsetting units on search filters would not actually remove them.',
        //        'Fixed a bug where resetting search fields on certain pages (i.e. stage and profile pages) would remove search constraints that should be uneditable.',
        //        'Improved search caching functionality and restored it to different parts of the website.'
        //    ]
        //},
        //{
        //    header: '02/03/18',
        //    lines: [
        //        'Added functionality to show similar teams when editing or creating teams to hopefully cut down on duplicates.'
        //    ]
        //},
        //{
        //    header: '01/29/18',
        //    lines: [
        //        'Updated the look-and-feel of Team Boxes to resolve a few display bugs and make better use of the space.',
        //        'Turned down the "fuzziness" of name searching to make it easier to find specific things.',
        //        'Document titles and meta tags will now update for each page, making link sharing better.',
        //        'User profile links are now based on user name rather than Id to make profile sharing easier.',
        //    ]
        //},
        //{
        //    header: '01/26/18',
        //    lines: [
        //        'Users can now bookmark teams.',
        //        'Added new "generic unit" feature for teams, allowing users to suggest a class/type/role instead of having to pick a unit.',
        //        'Added unit search parameters to teams. These will also apply to generic units.',
        //        'Made name searching "fuzzy" to make things easier to find.',
        //        'Leader searching will now extend to friend captain as well. This can be disabled.',
        //        'Updated search interfaces to use game icons instead of text.',
        //        'Removed info button from unit portraits. Clicking the portrait will now go to their DB page instead.',
        //        'Updated valdation rules for teams.',
        //        'Removed team search caching. This may be re-added in the future.',
        //        'Fixed an issue where importing from the calculator could put units in the wrong slots.',
        //        'Fixed an issue where users could not save a new team as a draft.',
        //        'Fixed an issue where users could not view their drafts.',
        //        'Fixed an issue where alerts might not be dismissed automatically.',
        //        'Strings will now be trimmed when submitted to the server.',
        //        'Added "Beta Tester" role and some features that only appear to people in it. ;)',
        //    ]
        //},
        //{
        //    header: '01/22/18',
        //    lines: [
        //        'Added sorting features to Stage and Team searches.',
        //        'Updated search recalling logic to be a little more intuitive.'
        //    ]
        //},
        //{
        //    header: '01/21/18',
        //    lines: [
        //        'Added unit aliases from OPTC-db to make searching easier. (i.e. you can now find Golden Pound Usopp by searching "GPU")',
        //        'Synced units and ships with the latest data from OPTC-db.',
        //        'Updated search field names to be a little more user-friendly.',
        //        '"Name" fields will now be auto-focused when opening search panels.',
        //        'Your last search will be recalled when opening search panels.',
        //        'Users will automatically upvote their own team after submitting it.',
        //        'Added markdown formatting guide.',
        //        'Increased team guide character limit to 40,000.',
        //        'Increased team credit character limit to 2,000.',
        //        'The latest news will now be shown when visiting the site.'
        //    ]
        //}, {
        //    header: '01/20/18 - Update #2',
        //    lines: [
        //        'Updated nakama.network to redirect to www.nakama.network to prevent login issues.',
        //    ]
        //}, {
        //    header: '01/20/18',
        //    lines: [
        //        'Added "stages" section so users can easily find teams for each stage.',
        //    ]
        //}, {
        //    header: '01/19/18 - Hotfix #1',
        //    lines: [
        //        'Fixed privacy policy on registration page linking to ToS',
        //        'Reduced minimum username length to 5 characters.',
        //        'Updated forms throughout the site to have better flow control with the enter key.',
        //        'Updated a few checkboxes to have larger cursor targets.'
        //    ]
        //}, {
        //    header: '01/19/18',
        //    lines: [
        //        'Added links to OPTC-db for unit details.',
        //        'Added "New" and "Trending" team lists on front page.',
        //        'Moved News and About to their own pages.',
        //        'Updated default page sizes.',
        //        'Fixed an issue where rapid submitting would not be throttled properly.',
        //    ]
        //},
        //{
        //    header: '01/17/18',
        //    lines: [
        //        'Updated Alert, Home Page, Socket, and Pagination UI.',
        //        'Added better login and logout flow.',
        //        'Added heartbeat to ensure the user has not lost their login session.',
        //    ]
        //}, {
        //    header: '01/16/18',
        //    lines: [
        //        'Added support for sockets on teams.',
        //        'Expanded "Free to Play" team search capabilities.',
        //        'Fixed Captain search to only apply to the Captain slot instead of Friend Captain and Captain.',
        //        'Added team flags to the team search views.',
        //    ]
        //}, {
        //    header: '01/13/18',
        //    lines: [
        //        "Added a 'draft' option for teams, allowing you to save a team without making it public.",
        //    ]
        //}, {
        //    header: '01/11/18',
        //    lines: [
        //        'Added support for markdown in guides and credits.',
        //    ]
        //}, {
        //    header: '01/10/18',
        //    lines: [
        //        'Added better error logging for server-side issues.',
        //        'Fixed an issue with search model caching.',
        //        'Fixed an issue where unauthorized users could try and submit teams.',
        //        'Fixed some issues with querying on different browsers.',
        //        'Added calculator export button on Team Detail page.',
        //        'Contributors can now add videos to teams. Videos uploaded by the team owner will be emphasized. Videos uploaded by other users will be smaller.',
        //        'Added Reddit, Discord, and Twitch login integration.',
        //    ]
        //}, {
        //    header: '01/09/18',
        //    lines: [
        //        'First release candidate has been launched!',
        //    ]
        //}
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
                message += '<li><em><a href="/news">Read More...</a></em></li>';
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