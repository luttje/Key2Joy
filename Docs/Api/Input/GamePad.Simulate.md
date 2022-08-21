# `GamePad.Simulate` (`GamePadControl`, `PressState`)


Simulate pressing or releasing (or both) gamepad buttons.


## Parameters

* **control (`GamePadControl`)** 

	Button to simulate

* **pressState (`PressState`)** 

	Action to simulate


## Examples

Shows how to press "A" on the gamepad for 500ms, then release it.

```js
GamePad.Simulate(GamePadControl.A, PressState.Press);
setTimeout(function () {
    GamePad.Simulate(GamePadControl.A, PressState.Release);
}, 500);
```