define(['plugins/http'], function (http) {
    return {
        buildSecurityHeaders: function (accessToken) {
            var headers = {};
            accessToken = accessToken || localStorage["accessToken"];
            if (accessToken) {
                headers["Authorization"] = "Bearer " + accessToken;
            }
            return headers;
        },
        query: function (endpoint, data, headers, func) {
            headers = headers || self.buildSecurityHeaders();
            data = data || {};
            return func(endpoint, data, headers);
        },
        get: function(endpoint, data, headers) {
            return this.query(endpoint, data, headers, http.get);
        },
        put: function (endpoint, data, headers) {
            return this.query(endpoint, data, headers, http.put);
        },
        post: function (endpoint, data, headers) {
            return this.query(endpoint, data, headers, http.post);
        },
        delete: function (endpoint, data, headers) {
            return this.query(endpoint, data, headers, http.delete);
        }
    };
});