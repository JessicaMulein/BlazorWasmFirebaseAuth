# Blazor WASM Firebase
C# .Net 6 Blazor WASM standalone with firebase auth

## Instructions
* <b>Note:</b> This repo will not run on its own without doing one of the following things:
  * Option 1: Use in place after modifying appsettings in order to test basic authentication
  * Option 2: Fork and start your own project
    * Fork Blazor-WASM-Firebase
    * Use as a template to build your own project. Rename the project/namespace.
  * Option 3: Start from scratch and just add the modules and few necessary view files
    * Create a directory structure like this
       ```
       use the pattern: $ mkdir -p {project}\src
       eg
       $ mkdir -p domino-train\src
       or
       $ mkdir -p domino-train\src

       {project}\
           |
	   +------- src\
	             |
		     +----\{project name or project namespace}
       ```
    * In project\src, Create a new Dotnet Blazor WASM SPA project using the Visual Studio wizard or dotnet new.
    * Copy .gitmodules from Blazor-WASM-Firebase to the toplevel
    * git add .gitsubmodules
    * git submodule update --init --recursive
    * Add project reference to Microsoft.Identity.Firebase and a build order dependency on firebase-js
    * git commit -m "add submodules"
    * In the blazor webassembly project, add the <FirebaseAuth /> tag to MainLayout
    * add the firebase-js-bundle.js reference to your wwwroot/index.html scripts
      ```
        <!-- existing section -->
        <script src="_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js"></script>
        <script src="_framework/blazor.webassembly.js"></script>
        <script>navigator.serviceWorker.register('service-worker.js');</script>
        <!-- add \/ this one -->
        <script src="firebase-js-bundle.js"></script>
      ```
    * Update Shared\LoginDisplay to use the FirebaseAuth.CurrentUser.BestAvailableName instead of context.user.Identity.Name
      ```
      Hello, @FirebaseAuth.CurrentUser!.BestAvailableName!
      ```
    * Copy Pages\Account\Login.razor and Register.razor
    * Add to Program.cs: await builder.UseFirebaseAuthenticationAsync();
* Copy/make a note of values from https://console.firebase.google.com/ && https://console.cloud.google.com/
* Edit project\src\project-directory\wwwroot\appsettings.json and appsettings.Development.json

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
