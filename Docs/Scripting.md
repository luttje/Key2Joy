# ![](KeyToJoy/Graphics/Icons/icon32.png?raw=true) KeyToJoy - Scripting
Using action scripts you can bind a complex sequence of actions to a trigger.

Most actions that you can configure in KeyToJoy are available through scripting. To get started check out the examples, or browse the full API Reference.

* [📃 Scripting API Reference](Index.md)



## Available Scripting Languages
When writing action scripts you have the choice to use any of these languages:
* [Lua 5.2.3](https://www.lua.org/manual/5.2/)
* [ECMAScript 5.1 (Javascript)](https://262.ecma-international.org/5.1/) *(with partial [ECMAScript 2015 - 2022 Support](https://github.com/sebastienros/jint#version-3x))*

*If you're an advanced user and curious about the Lua and Javascript implementations: this project uses [NLua](https://github.com/NLua/NLua) and [Jint](https://github.com/sebastienros/jint).*


## Examples
> 🚧 Work in Progress: We need to clean these up by moving them to the relevant API documentation. Also I should write some generic examples here

**Lua Example:**

```lua
Print("test")

GamePad.Simulate(GamePadControl.A, PressState.PressAndRelease)
SetTimeout(function ()
   App.Command("abort")
end, 2000)

Print("end test")
```

**Javascript Example:**

```js
Print("test");

GamePad.Simulate(GamePadControl.A, PressState.PressAndRelease);
setTimeout(function () {
  App.Command("abort");
}, 2000);

Print("end test");
```

**Javascript example showing access to Windows:**

```js
let handles = Window.GetAll();

handles.forEach(function (handle) {
  Print(
    handle + " / " + Window.GetClass(handle) + " : " + Window.GetTitle(handle)
  );
});

Print(Window.GetForeground());
```
