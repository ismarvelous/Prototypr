# Prototypr
Status: under construction  
Goal: A prototyping engine inspired by Jekyll that is using Razor as a template engine and JSON and Markdown files as a datasource. Build on top of ASP.NET MVC.

## Routes and paths
Urls are handled as follows:  

For example "http://yourdomain/docs/lorem"

* template: /views/docs/item.cshtml; model: based on available data file /app_data/docs/lorem.(json|md)  
* template: /views/docs/lorem/index.cshtml | /views/docs/lorem.cshtml; model: When there is a data folder called /app_data/docs/lorem available the model is an IEnumerable of all available objects / files. Otherwise model is an empty object.

-------
You can overrule the chosen view/template any time by define a "layout" property (json) or meta data item (markdown) in your data file. Otherwise item.cshtml is used.

### LICENSE
Currently this software is licensed under: http://creativecommons.org/licenses/by-nc-sa/3.0/deed.nl

We use Markdown Sharp which is licensed under: MIT (https://code.google.com/p/markdownsharp/)