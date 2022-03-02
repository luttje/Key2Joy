# KeyToJoy
Simulate a GameController/Joystick using your keyboard and mouse.

This is an early version so I can play the game ['Aperture Desk
Job'](https://store.steampowered.com/app/1902490/Aperture_Desk_Job/).


**⚠ Use at own risk!** I got a blue screen after uninstalling the Scp
Virtual Bus Driver and restarting. Worse may happen to you.

## Usage

1. Download a binary from the [releases of this
   repo](https://github.com/luttje/KeyToJoy/releases)
2. Extract all files to the same location
3. Start KeyToJoy.exe
4. Accept installation of the Scp Virtual Bus Driver
5. Tick the checkbox to simulate the game controller


## Uninstalling

Use `ScpDriverInstaller.exe` to uninstall the driver! Do not uninstall it through Device manager or
you'll have trouble reinstalling it! (Like I have now)


## Credits 😍

This exists only because of this awesome NuGet package
([DavidRieman/SimWinInput](https://github.com/DavidRieman/SimWinInput))
which allows simulation of gamepads from .NET.

Simulation is made possible through installation and usage of the
[nefarius/ScpVBus](https://github.com/nefarius/ScpVBus) driver. 

Inspired by [JoyToKey](https://joytokey.net/en/) which does the inverse (simulate keyboard with gamepad).

## License

This software is license under [the MIT License (see
LICENSE-file)](LICENSE). The licenses for other libraries and/or code used
can be found in [the LICENSE-3RD-PARTY.txt file](LICENSE-3RD-PARTY.txt).