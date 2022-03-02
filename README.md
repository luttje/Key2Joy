# KeyToJoy
Simulate a GameController/Joystick using your keyboard and mouse.

This is an early version so I can play the game ['Aperture Desk
Job'](https://store.steampowered.com/app/1902490/Aperture_Desk_Job/).


## Usage

1. Download a binary from the [releases of this
   repo](https://github.com/luttje/KeyToJoy/releases)
2. Extract all files to the same location
3. Start KeyToJoy.exe
4. Accept installation of the Scp Virtual Bus Driver
5. Tick the checkbox to simulate the game controller


## Uninstalling

1. Simply remove all files you downloaded
2. Go to Device Manager (Windows + R > `devmgmt.msc`)
3. Open the `System Devices` category
4. Right click and uninstall `Scp Virtual Bus Driver`: ![ScpVBus driver
properties showing version 1.0.0.103 from 5-5-2013 by driver provider
Scarlet.Crush Productions](.github/device-manager-details.jpg)


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