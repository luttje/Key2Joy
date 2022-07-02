# `SetDelayedFunctions` (`Int64`, `Action[]`)


Execute functions whilst waiting the specified time between them.

The first function is executed immediately.


## Parameters

* **waitTime (`Int64`)** 

	Time to wait (in milliseconds) between function calls

* **callbacks (`Action[]`)** 

	One or more functions to execute


## Examples

Shows how to count down from 3 and execute a command using Lua.

```lua
SetDelayedFunctions(
   1000,
   function ()
      Print("Aborting in 3 second...")
   end,
   function ()
      Print("Three")
   end,
   function ()
      Print("Two")
   end,
   function ()
      Print("One")
   end,
   function ()
      App.Command("abort")
   end
)
```