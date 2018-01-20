export class NewsPage {
    news: NewsEntry[] = [{
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
    ]
}

export class NewsEntry {
    header: string;
    lines: string[];
}