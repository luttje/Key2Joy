# `ClearTimeout` (```TimeoutId```)

Cancels a timeout previously established by calling SetTimeout()

## Parameters
* **timeoutId (```TimeoutId```)** 
	Id returned by SetTimeout to cancel


## Examples
> Shows how to set and immediately cancel a timeout.
> 
> #### _js_:
> ```js
> var timeoutID = setTimeout(() => {
>    Print("You shouldn't see this because the timeout will have been cancelled!");
> }, 1000);
> 
> Print(timeoutID);
> 
> clearTimeout(timeoutID);
> ```
---
