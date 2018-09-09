/******************************
 * Nakama Network Integration *
 *     - by User:RoboCafaz    *
 ******************************/

/* --- Team Search --- *\
 HTML:
    <div class="nakama-teams"
        data-page-size="15"
        data-page="2"
        data-sort-by="Name"
        data-sort-desc="1"
        data-term="deany"
        data-submitted-by="nerd"
        data-leader-id="543"
        data-no-help="1"
        data-stage-id="777"
        data-invasion-id="666"
        data-global="1"
        data-free-to-play="2"
        data-classes="3"
        data-types="45"
    ></div>
 Template:
    {{Nakama Network Teams
     | PageSize    = 
     | Page        = 
     | SortBy      = 
     | SortDesc    = 
     | Term        = 
     | SubmittedBy = 
     | LeaderId    = 
     | NoHelp      = 
     | StageId     = 
     | InvasionId  = 
     | Global      = 
     | FreeToPlay  = 
     | Classes     = 
     | Types       = 
    }}
\* --- --- --- --- --- */

window.nakama = window.nakama || {
    errorMsg: '<div class="nakama-info nakama-error">'
    + '<img src="https://vignette.wikia.nocookie.net/onepiecetreasurecruiseglobal/images/b/ba/Poison_resitance.png" title="Error!" />'
    + '<span>An error occurred trying to load <a href="https://www.nakama.network" target="_blank">Nakama Network</a> teams...</span>'
    + '</div>',
    headerMsg: '<h1><a href="https://www.nakama.network" target="_blank">Nakama Network</a> Teams</h1>',
    footerMsg: '<div class="nakama-info">'
    + '<img src="https://vignette.wikia.nocookie.net/onepiecetreasurecruiseglobal/images/4/44/F1762.png" title="Nakama Network" />'
    + '<span>Visit <a href="https://www.nakama.network" target="_blank">Nakama Network</a> for more teams!</span>'
    + '</div>',
    loadingTemplate: '<div class="nakama-info nakama-loading">'
    + '<img src="https://vignette.wikia.nocookie.net/onepiecetreasurecruiseglobal/images/9/9e/Auto-heal.png" title="Loading..." />'
    + '<span>Loading <a href="https://www.nakama.network" target="_blank">Nakama Network</a> teams...</span>'
    + '</div>',
    blankImage: 'https://onepiece-treasurecruise.com/wp-content/themes/onepiece-treasurecruise/images/noimage.png',
    getUnitImage: function (unitId) {
        if (unitId < 0) {
            return nakama.blankImage;
        }
        unitId = '' + unitId;
        while (unitId.length < 4) {
            unitId = '0' + unitId;
        }
        return 'https://onepiece-treasurecruise.com/wp-content/uploads/f' + unitId + '.png';
    },
    buildUrl: function (container, url, attribute, parameter) {
        var value = undefined;
        var arg = container.attr('data-' + attribute);
        if(arg !== undefined && arg !== '') {
            value = arg;
        }
        if(value !== undefined) {
            url += (url.indexOf('?') === -1 ? '?' : '&') + parameter + '=' + encodeURIComponent(value);
        }
        return url;
    },
    buildHtml: function (team) {
        var html = '<div class="nakama-team">'
                 + '<table>'
                 + '  <thead>'
                 + '    <tr>'
                 + '      <th colspan="5">'
                 + '        <a href="https://www.nakama.network/teams/' + team.id + '" target="_blank">' + team.name + '</a>'
                 + '      </th>'
                 + '      <td class="nakama-score ' + (team.score > 0 ? 'positive' : 'negative') + '">' 
                 + '        <span title="Score">' + team.score + '</span>'
                 + '      </td>'
                 + '    </tr>'
                 + '  </thead>'
                 + '  <tbody>'
                 + '    <tr>';
        for(var i = 0 ; i < 6 ; i++) {
            var unit = $.grep(team.teamUnits, function (teamUnit) {
                return teamUnit.position == i;
            });
            if (unit.length > 0) {
                unit = unit[0];
            } else {
                unit = { position: i, unitId: -1, name: '' };
            }
            html += '  <td class="unit unit-' + i +'">'
            if(unit.id < 0) {
                html += ' <img src="' + nakama.blankImage + '" />';
            } else {
                html += '    <a href="/wiki/' + encodeURIComponent(unit.unitName) + '">'
                      + '      <img src="' + nakama.getUnitImage(unit.unitId) + '" onerror="if(this.src != nakama.blankImage) this.src = nakama.blankImage" title="' + unit.unitName + '" />'
                      + '    </a>';
            }
            html += '  </td>';
        }
        html += '  </tr>';
        html += '  <tr>'
              + '    <td class="stage" colspan="' + (team.invasionId ? 3 : 6) + '">'
              + '      <a href="/wiki/' + encodeURIComponent(team.stageName) + '" target="_blank">' + team.stageName + '</a>'
              + '    </td>'
        if(team.invasionId) {
            html  += '    <td class="invasion" colspan="3">'
                   + '      <a href="/wiki/' + encodeURIComponent(team.invasionName) + '" target="_blank">' + team.invasionName + '</a>'
                   + '    </td>'
        }
        html += '  </tr>'
              + '  <tr>'
              + '    <td class="ship"  colspan="3">'
              + '      <a href="/wiki/' + encodeURIComponent(team.shipName) + '" target="_blank">' + team.shipName + '</a>'
              + '    </td>'
              + '    <td class="author" colspan="3">'
              + '      <a href="https://www.nakama.network/profile/' + encodeURIComponent(team.submittedByName) + '" target="_blank">' + team.submittedByName + '</a>'
              + '    </td>'
              + '  </tr>'
              + '  </tbody>'
              + '</table>'
              + '</div>';
        return html;
    },
    reportNothing: function (container) {
        container.html(nakama.footerMsg);
    },
    reportFailure: function (container) {
        container.html(nakama.errorMsg);
    },
    loadTeams: function () {
        var container = $(this);
        container.html(nakama.loadingTemplate);
        var url = 'https://www.nakama.network/api/team/wiki';
        url = nakama.buildUrl(container, url, 'page-size',    'pageSize');
        url = nakama.buildUrl(container, url, 'page',         'page');
        url = nakama.buildUrl(container, url, 'sort-by',      'sortBy');
        url = nakama.buildUrl(container, url, 'sort-desc',    'sortDesc');
        url = nakama.buildUrl(container, url, 'term',         'term');
        url = nakama.buildUrl(container, url, 'submitted-by', 'submittedBy');
        url = nakama.buildUrl(container, url, 'leader-id',    'leaderId');
        url = nakama.buildUrl(container, url, 'no-help',      'noHelp');
        url = nakama.buildUrl(container, url, 'leader-id',    'leaderId');
        url = nakama.buildUrl(container, url, 'stage-id',     'stageId');
        url = nakama.buildUrl(container, url, 'invasion-id',  'invasionId');
        url = nakama.buildUrl(container, url, 'global',       'global');
        url = nakama.buildUrl(container, url, 'free-to-play', 'freeToPlay');
        url = nakama.buildUrl(container, url, 'classes',      'classes');
        url = nakama.buildUrl(container, url, 'types',        'types');
        var stageId = container.attr('data-stage-id');
        $.getJSON(url).done(function (data) {
            var sets = $.map(data.results, nakama.buildHtml);
            var html = nakama.headerMsg
                     + sets.join('')
                     + nakama.footerMsg;
            container.html(html);
        }).fail(function (error) {
            nakama.reportFailure(container);
        });
    },
    process: function ($content) {
        if ($content) {
            $content.find('.nakama-teams').each(nakama.loadTeams);
        }
    }
};

importArticle({
    type: 'style',
    article: 'MediaWiki:NakamaNetwork/NakamaNetwork.css'
});
mw.hook('wikipage.content').add(nakama.process);