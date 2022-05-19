# Blazor WASM Firebase
C# .Net 6 Blazor WASM standalone with firebase auth

## Instructions
* Copy values from https://console.firebase.google.com/ && https://console.cloud.google.com/
* Edit src/firebase-js/src/firebase-js.js
* Edit src/blazor-wasm-firebase/wwwroot/appsettings.json and appsettings.Development.json
* Test authentication

## To copy to another project/replicate the main things are:
* copy npjms and add subproject src/firebase-js
* copy add and reference c# subproject src/Microsoft.Identity.Firebase from your blazor webassembly project
* in the blazor webassembly project, add the <FirebaseAuth /> tag to MainLayout
* add the firebase-js-bundle.js reference to your wwwroot/index.html scripts
* Update LoginDisplay to use the FirebaseAuth.CurrentUser.BestAvailableName instead of context.user.Identity.Name
* Add to Program.cs: await builder.UseFirebaseAuthenticationAsync();

## References
* Active demo project https://github.com/FreddieMercurial/Domino-Train work in progress, driving changes to this repo
### blazor
* https://code-maze.com/blazor-webassembly-authentication-aspnetcore-identity/
* https://code-maze.com/authenticationstateprovider-blazor-webassembly/
* https://github.com/CodeMazeBlog/blazor-series
* https://social.technet.microsoft.com/wiki/contents/articles/52576.blazor-deploying-an-application-on-firebase.aspx
* https://github.com/cschweig2/TrailBlazor
* https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-identity-server?view=aspnetcore-3.1&tabs=visual-studio
* https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/?view=aspnetcore-3.1
* https://github.com/nh43de/HostedBlazorWebAssemblyWithFirebase
* https://github.com/HugoRheaume/H21-609-Equipe04
### misc
* https://www.c-sharpcorner.com/article/asp-net-core-blazor-simple-game-development-using-net-core-3-0-preview-web-api/?msclkid=10f31adacee811ec8c09f9cbd6eab264

Quite a few more!
