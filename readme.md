# Prerequisites

* [Node.JS](https://nodejs.org/en/)
* Visual Studio capable of compiling ASP.NET Core applications (2017 recommended)
* [Typewriter extension for Visual Studio](https://github.com/frhagn/Typewriter)

# Building the Web App

* Navigate to Source/TreasureGuide.Web
* Open a command window and run the following commands:
* npm install -g aurelia-cli
* npm install
* gulp build-dev

Re-run gulp build-dev whenever you need to update the webapp.

You can also execute this from Visual Studio by opening the Task Runner Explorer window, which will allow you to run all the gulp tasks too!

# Deploying the database locally

To run the site on your local box, you'll first need to create a database instance. To do so, simply open a command window and run the following:
 * sqllocaldb create "tcdb"
 
This will deploy a local MSSSQL instance called "(localdb)\tcdb"

Once that has been created, open up the TreasureGuide solution and navigate to the TreasureGuide.DB project.
Under there you'll see a _local.publish.xml file. Right-click it and choose "Publish..." then confirm the following popup.
This will create the database in your (localdb)\tcdb instance and populate it with whatever snapshot is currently captured in the repository.

If you want to update the database with the latest units, stages, etcetera, you can run the TreasureGuide.Sniffer project which will re-populate
the database with all the latest units scraped from our friends at [OPTC-db](https://optc-db.github.io/) and [OPTC Voyage Log](https://www.reddit.com/user/zl1814)!

# User Secrets

To run the app locally you'll need to set your user secrets. To do so, right-click the TreasureGuide.Web application and select "Manage User Secrets", then fill out the form with the following:

````
{
    "Authentication:Jwt:Key": "<Write random long gibberish here.>",
    "Authentication:Google:ClientID": "",
    "Authentication:Google:ClientSecret": "",
    "Authentication:Facebook:ClientID": "",
    "Authentication:Facebook:ClientSecret": "",
    "Authentication:Twitter:ConsumerKey": "",
    "Authentication:Twitter:ConsumerSecret": "",
    "Authentication:Reddit:ClientID": "",
    "Authentication:Reddit:ClientSecret": "",
    "Authentication:Twitch:ClientID": "",
    "Authentication:Twitch:ClientSecret": "",
    "Authentication:Discord:ClientID": "",
    "Authentication:Discord:ClientSecret": ""
}
````

You only need one application to test locally with, but all the entries must be there. The oauth redirect is https://<your app url>/signin-<client> (i.e. /signin-facebook, /signin-google, etc)

# Chrome Debugging

I recommend setting this flag to reduce pain: chrome://flags/#allow-insecure-localhost