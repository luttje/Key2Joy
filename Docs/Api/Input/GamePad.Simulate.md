# `GamePad.Simulate` (`GamePadControl`, `PressState`, `Int32`)


Simulate pressing or releasing (or both) gamepad buttons.


## Parameters

* **control (`GamePadControl`)** 
	Button to simulate

* **pressState (`PressState`)** 
	Action to simulate

* **gamepadIndex (`Int32`)** 
	Which of 4 possible gamepads to simulate: 0 (default), 1, 2 or 3


## Examples

> Shows how to press "A" on the gamepad for 500ms, then release it.
> 
> #### _js_:
> ```js
> GamePad.Simulate(GamePadControl.A, PressState.Press);
> setTimeout(function () {
>     GamePad.Simulate(GamePadControl.A, PressState.Release);
> }, 500);
> ```
---
