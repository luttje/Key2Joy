# `AppCommand`

The possible commands to run in the app.

## Example Use
`AppCommand.Abort`

## Possible Values
* `Abort`: Aborts listening for triggers
* `ResetScriptEnvironment`: Recreate the scripting environment (loses all variables, functions and other changes scripts made)
* `ResetMouseMoveTriggerCenter`: Can be called to reset the cursor to the center. Useful for games that take control of the cursor
and place it somewhere else.
