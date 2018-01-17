export class HomePage {
    news: NewsEntry[] = [
        {
            header: '01/17/18',
            lines: [
                'Updated Alert, Home Page, Socket, and Pagination UI.',
                'Added better login and logout flow.',
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