# Scripting API Reference

## All Enumerations

* [`AppCommand`](Api/Enumerations/AppCommand.md)
* [`GamePadControl`](Api/Enumerations/GamePadControl.md)
* [`KeyboardKey`](Api/Enumerations/KeyboardKey.md)
* [`PressState`](Api/Enumerations/PressState.md)

## Input

* [`GamePad.Simulate` (`GamePadControl`, `PressState`)](Api/Input/GamePad.Simulate.md)
* [`Keyboard.GetKeyDown` (`KeyboardKey`)](Api/Input/Keyboard.GetKeyDown.md)
* [`Keyboard.Simulate` (`KeyboardKey`, `PressState`)](Api/Input/Keyboard.Simulate.md)

## Logic

* [`App.Command` (`AppCommand`)](Api/Logic/App.Command.md)
* [`ClearInterval` (`IntervalId`)](Api/Logic/ClearInterval.md)
* [`ClearTimeout` (`TimeoutId`)](Api/Logic/ClearTimeout.md)
* [`SetDelayedFunctions` (`Int64`, `Action[]`)](Api/Logic/SetDelayedFunctions.md)
* [`SetInterval` (`CallbackAction`, `Int64`, `Object[]`)](Api/Logic/SetInterval.md)
* [`SetTimeout` (`CallbackAction`, `Int64`, `Object[]`)](Api/Logic/SetTimeout.md)

## Windows

* [`Window.Find` (`String`, `String`)](Api/Windows/Window.Find.md)
* [`Window.GetAll` ()](Api/Windows/Window.GetAll.md)
* [`Window.GetClass` (`IntPtr`)](Api/Windows/Window.GetClass.md)
* [`Window.GetForeground` ()](Api/Windows/Window.GetForeground.md)
* [`Window.GetTitle` (`IntPtr`)](Api/Windows/Window.GetTitle.md)

