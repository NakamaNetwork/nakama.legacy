# Prerequisites

* Node.JS
* Visual Studio capable of compiling ASP.NET Core applications.
* [Typewriter extension for Visual Studio](https://github.com/frhagn/Typewriter)

# Building the Web App

* Navigate to Source/TreasureGuide.Web
* Open a command window and run the following commands:
* npm install -g aurelia-cli
* npm install
* gulp build-dev

Re-run gulp build-dev whenever you need to update the webapp.

# User Secrets

To run the app locally you'll need to set your user secrets. To do so, right-click the TreasureGuide.Web application and select "Manage User Secrets", then fill out the form with the following:

````
{
    "Authentication:Jwt:Key": "<Insert key for JWT access keys>",
    "Authentication:Google:ClientID": "<Client ID for Google Login>",
    "Authentication:Google:ClientSecret": "<Client Secret>",
    "Authentication:Facebook:ClientID": "<Client ID for Facebook Login",
    "Authentication:Facebook:ClientSecret": "<Client Secret>",
    "Authentication:Twitter:ConsumerKey": "<Consumer Key for Twitter Login>",
    "Authentication:Twitter:ConsumerSecret": "<Consumer Secret>"
}
````

You only need one application to test locally with. The oauth redirect is https://<your app url>/signin-<client> (i.e. /signin-facebook, /signin-google, etc)

# Chrome Debugging

I recommend setting this flag to reduce pain: chrome://flags/#allow-insecure-localhost