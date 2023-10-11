# `ClearInterval` (`IntervalId`)


Cancels an interval previously established by calling SetInterval()


## Parameters

* **intervalId (`IntervalId`)** 
	Id returned by SetInterval to cancel


## Examples

> Shows how to count up to 3 every second and then stop by using ClearInterval();
> 
> #### _js_:
> ```js
> var count = 0;
> var intervalId;
>             
> intervalId = setInterval(() => {
>    Print(count++);
>             
>    if(count == 3)
>       clearInterval(intervalId);
> }, 1000);
>             
> Print(intervalId);
> ```
---
