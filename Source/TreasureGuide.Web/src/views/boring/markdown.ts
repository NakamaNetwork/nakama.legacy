export class MarkdownPage {
    guides = [{
        header: 'Line Breaks',
        text: 'For markdown, you\'ll need to add two spaces at the end of your line  \r\n' +
        'followed by a line-break to create a proper line-break.' +
        'to create a proper line break.\r\n' +
        '\r\n' +
        'Otherwise it will just be\r\n' +
        'a run-on sentence.\r\n' +
        '\r\n' +
        'Double-spacing your\r\n' +
        '\r\n' +
        'lines will work as expected.'
    }, {
        header: 'Text Styling',
        text: 'Emphasis, aka italics, with *asterisks* or _underscores_.\r\n' +
        '\r\n' +
        'Strong emphasis, aka bold, with **asterisks ** or __underscores__.\r\n' +
        '\r\n' +
        'Combined emphasis with **asterisks and _underscores_**.\r\n' +
        '\r\n' +
        'Strikethrough uses two tildes. ~~Scratch this.~~'
    }, {
        header: 'Headers',
        text: '# H1\r\n' +
        '## H2\r\n' +
        '### H3\r\n' +
        '#### H4\r\n' +
        '##### H5\r\n' +
        '###### H6'
    }, {
        header: 'Lists',
        text: '1. First ordered list item\r\n' +
        '2. Another item\r\n' +
        '  * Unordered sub-list. Note the two leading spaces.\r\n' +
        '1. Actual numbers don\'t matter, just that it\'s a number\r\n' +
        '  1. Ordered sub-list. Note the two leading spaces.\r\n' +
        '4. And another item.\r\n' +
        '\r\n' +
        '  Indented text. Note the two leading spacecs.' +
        '\r\n' +
        '* Unordered list can use asterisks\r\n' +
        '- Or minuses\r\n' +
        '+ Or pluses'
    }, {
        header: 'Links',
        text: '[I\'m an inline-style link](https://www.reddit.com/r/OnePieceTC/)\r\n' +
        '\r\n' +
        '[I\'m an inline-style link with title](https://www.gamefaqs.com/boards/123874-one-piece-treasure-cruise "OPTC on GameFAQs")\r\n' +
        '\r\n' +
        '[I\'m a reference-style link][Arbitrary case-insensitive reference text]\r\n' +
        '\r\n' +
        '[You can use numbers for reference-style link definitions][1]\r\n' +
        '\r\n' +
        'Or leave it empty and use the [link text itself].\r\n' +
        '\r\n' +
        'Some text to show that the reference links can follow later.\r\n' +
        '\r\n' +
        '[arbitrary case-insensitive reference text]: https://www.gamefaqs.com/boards/123874-one-piece-treasure-cruise\r\n' +
        '[1]: https://www.facebook.com/treasurecruise.onepiece/\r\n' +
        '[link text itself]: http://www.reddit.com/r/OnePieceTC'
    }, {
        header: 'Code',
        text: 'Inline `code` has `back-ticks around` it.\r\n' +
        '\r\n' +
        '```\r\n' +
        'var s = "Block code";\r\n' +
        'alert(s);\r\n' +
        '```'
    }, {
        header: 'Tables',
        text: 'Colons can be used to align columns.\r\n' +
        '\r\n' +
        '| Tables        | Are           | Cool  |\r\n' +
        '| ------------- |:-------------:| -----:|\r\n' +
        '| col 3 is      | right-aligned | $1600 |\r\n' +
        '| col 2 is      | centered      |   $12 |\r\n' +
        '| zebra stripes | are neat      |    $1 |\r\n' +
        '\r\n' +
        'There must be at least 3 dashes separating each header cell.\r\n' +
        'The outer pipes (|) are optional, and you don\'t need to make the \r\n' +
        'raw Markdown line up prettily. You can also use inline Markdown.\r\n' +
        '\r\n' +
        'Markdown | Less | Pretty\r\n' +
        '--- | --- | ---\r\n' +
        '*Still* | `renders` | **nicely**\r\n' +
        '1 | 2 | 3'
    }, {
        header: 'Block Quotes',
        text: '> Blockquotes are very handy in email to emulate reply text.\r\n' +
        '> This line is part of the same quote.\r\n' +
        '\r\n' +
        'Quote break.\r\n' +
        '\r\n' +
        '> This is a very long line that will still be quoted properly when it wraps. Oh boy let\'s keep writing to make sure this is long enough to actually wrap for everyone.Oh, you can * put * **Markdown ** into a blockquote.'
    }
    ];
}