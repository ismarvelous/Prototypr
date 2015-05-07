[![Build status](https://ci.appveyor.com/api/projects/status/dxobndb6jqs0y3js?svg=true)](https://ci.appveyor.com/project/ismarvelous/prototypr)  

# Prototypr
Prototypr is a prototyping engins using Razor as a template engine. Prototypr.Files is an implementation of the core using JSON and Markdown files as a datasource. Prototypr is currently UNDERCONSTRUCTION!  

Goal: Prototypr, where front- and backend developers come along! Razor, JSON and Markdown are easy readable and understandable for both, frontend and backend developers. With Prototypr you can easily mock your cms data (contenttypes, pagetypes, documenttypes, etc) with JSON and Markdown. And build your templates by using Razor.
Prototypr.Blog is an implementation of a lightweight blogengine using Prototypr.Files.

## Routes and paths
Urls are handled as follows:  

For example "http://yourdomain/docs/lorem"

* Is using ~/Views/docs/lorem.cshtml or ~/Views/docs/lorem/index.cshtml as the template
* Is using ~App_data/docs/lorem, ~App_data/docs/lorem.md or ~App_data/docs/lorem.json as datasource
  
TIP: You can overrule the chosen template any time by define a "Layout" property (json) or YAML item (markdown) in your data file.  

## Start and Publish  
Currently we are working on two nuget packages

* Prototypr.Blog: a starter project for blogger which is using Prototypr.Files (a file based CMS solution)
* Prototypr: A nuget package of the core. This package also contains Prototypr.Files

### Plugins?
Not jet possible....  
To be continued :-)  

### Static file generation
Static file (.html) generation is not yet possible. We are working hard to make this possible in the very near future.

### LICENSE
Currently this software is licensed under: http://creativecommons.org/licenses/by-nc-sa/3.0/deed.nl  
We use Markdown Sharp which is licensed under: MIT (https://code.google.com/p/markdownsharp/)