# ![](KeyToJoy/Graphics/Icons/icon32.png?raw=true) KeyToJoy - Scripting

> 🚧 TODO: Full scripting reference, preferably generated from source-code info so it's always up-to-date

**Lua Example:**
```lua
print("test")

gamepad(GamePadControl.A, PressState.PressAndRelease)
wait(function()
   app_command("abort")
end, 2000)

print("end test")
```

**Javascript Example:**
```js
print("test")

gamepad(GamePadControl.A, PressState.PressAndRelease)
wait(function() {
   app_command("abort")
}, 2000)

print("end test")
```
