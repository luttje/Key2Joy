# ![](KeyToJoy/Graphics/Icons/icon32.png?raw=true) KeyToJoy - Scripting
Using action scripts you can bind a complex sequence of actions to a trigger.

Most actions that you can configure in KeyToJoy are available through scripting. To get started check out the examples, or browse the full API Reference.

* [📃 Scripting API Reference](Index.md)



## Available Scripting Languages
When writing action scripts you have the choice to use any of these languages:
* [Lua 5.2.3](https://www.lua.org/manual/5.2/)
* [ECMAScript 5.1 (Javascript)](https://262.ecma-international.org/5.1/)

If you're an advanced user and are running into limitations. These are the implementations we use: [NLua](https://github.com/NLua/NLua) and [Jint](https://github.com/sebastienros/jint)


## Examples
> 🚧 Work in Progress: We need to clean these up by moving them to the relevant API documentation. Also I should write some generic examples here

**Lua Example:**

```lua
print("test")

gamepad(GamePadControl.A, PressState.PressAndRelease)
wait(function ()
   app_command("abort")
end, 2000)

print("end test")
```

**Javascript Example:**

```js
Print("test");

Gamepad(GamePadControl.A, PressState.PressAndRelease);
Wait(function () {
  AppCommand("abort");
}, 2000);

Print("end test");
```

**Javascript example showing access to Windows:**

```js
let handles = WindowGetAll();

handles.forEach(function (handle) {
  Print(
    handle + " / " + WindowGetClass(handle) + " : " + WindowGetTitle(handle)
  );
});

Print(WindowGetForeground());
```