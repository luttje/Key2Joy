# `SetInterval` (`CallbackAction`, `Int64`, `Object[]`)


Repeatedly calls a function or executes a code snippet, with a fixed time delay between each call


## Parameters

* **callback (`CallbackAction`)** 

	Function to execute after each wait

* **waitTime (`Int64`)** 

	Time to wait (in milliseconds)

* **arguments (`Object[]`)** 

	Zero or more extra parameters to pass to the function


## Examples

Shows how to count up to 10 every second and then stop by using ClearInterval();

```js
setTimeout(function () {
  Print("Aborting in 3 second...")
   
  setTimeout(function () {
    Print("Three")

    setTimeout(function () {
      Print("Two")

      setTimeout(function () {
        Print("One")

        setTimeout(function () {
          App.Command("abort")
        }, 1000)
      }, 1000)
    }, 1000)
  }, 1000)
}, 1000)
```