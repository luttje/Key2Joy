# `Util.PathExpand` (```String```)

Expands system environment variables (See also: https://ss64.com/nt/syntax-variables.html).

## Parameters
* **path (```String```)** 
	The path to expand

## Returns
```String```
String containing expanded path.

## Examples
> Demonstrates how to get the home drive
> 
> #### _js_:
> ```js
> Print(Util.PathExpand("%HOMEDRIVE%/"))
> ```
---
