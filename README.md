# Prototypr
What is Prototypr: A prototyping engine inspired by engines like Jekyll and Mixture. Prototypr is using Razor as a template engine and JSON and Markdown files as a datasource. Prototypr is currently UNDERCONSTRUCTION!  

Goal: Prototypr, where front- and backend developers come along! Razor, JSON and Markdown are easy readable and understandable for both, frontend and backend developers. With Prototypr you can easily mock your cms data (contenttypes, pagetypes, documenttypes, etc) with JSON and Markdown. And build your templates by using Razor.

## Routes and paths
Urls are handled as follows:  

For example "http://yourdomain/docs/lorem"

* Is using ~/Views/docs/lorem.cshtml or ~/Views/docs/lorem/index.cshtml as the template
* Is using ~App_data/docs/lorem, ~App_data/docs/lorem.md or ~App_data/docs/lorem.json as datasource
  
TIP: You can overrule the chosen template any time by define a "Layout" property (json) or YAML item (markdown) in your data file.  


## Start and Publish  
Download Visual Studio Express and open this project.  You can download VS express for web here: http://www.microsoft.com/visualstudio/eng/products/visual-studio-express-products#d-express-for-web  
TIP: Use windows azure and a publish profile or even continuous delivery in tfs online to publish your work continously. Check: http://www.windowsazure.com/en-us/develop/net/common-tasks/publishing-with-tfs/ for more information about continuous delivery.

### Plugins?
Not jet possible....  
To be continued :-)  

### Static file generation
Static file (.html) generation is not yet possible. We are working hard to make this possible in the very near future.

### LICENSE
Currently this software is licensed under: http://creativecommons.org/licenses/by-nc-sa/3.0/deed.nl  
We use Markdown Sharp which is licensed under: MIT (https://code.google.com/p/markdownsharp/)