# `SetInterval` (```CallbackAction```, ```Int64```, ```Object[]```)

Repeatedly calls a function or executes a code snippet, with a fixed time delay between each call

## Parameters
* **callback (```CallbackAction```)** 
	Function to execute after each wait

* **waitTime (```Int64```)** 
	Time to wait (in milliseconds)

* **arguments (```Object[]```)** 
	Zero or more extra parameters to pass to the function

## Returns
```IntervalId```
An interval id that can be removed with clearInterval.

## Examples
> Shows how to count every second
> 
> #### _js_:
> ```js
> let counter = 0;
> setInterval(function () {
>     counter++;
>     Print(counter);
> }, 1000)
> ```
---
