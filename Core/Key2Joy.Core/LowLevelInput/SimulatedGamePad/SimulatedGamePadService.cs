using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Key2Joy.LowLevelInput.XInput;
using SimWinInput;

namespace Key2Joy.LowLevelInput.SimulatedGamePad;

/// <inheritdoc />
public class SimulatedGamePadService : ISimulatedGamePadService
{
    private const int MAX_GAMEPADS = 4;

    private readonly ISimulatedGamePad[] gamePads;

    public SimulatedGamePadService(ISimulatedGamePad[] gamePads = null)
    {
        if (gamePads != null)
        {
            this.gamePads = gamePads;
            return;
        }

        this.gamePads = new ISimulatedGamePad[MAX_GAMEPADS];

        for (var index = 0; index < MAX_GAMEPADS; index++)
        {
            this.gamePads[index] = new SimulatedGamePad(index);
        }
    }

    /// <inheritdoc />
    public void Initialize()
        => SimGamePad.Instance.Initialize();

    /// <inheritdoc />
    public void ShutDown()
        => SimGamePad.Instance.ShutDown();

    /// <inheritdoc />
    public ISimulatedGamePad GetGamePad(int gamePadIndex)
        => this.gamePads[gamePadIndex];

    /// <inheritdoc />
    public ISimulatedGamePad[] GetAllGamePads(bool onlyPluggedIn = true)
    {
        if (onlyPluggedIn)
        {
            return this.gamePads.Where(gamePad => gamePad.GetIsPluggedIn()).ToArray();
        }

        return this.gamePads;
    }

    /// <inheritdoc />
    public IList<IGamePadInfo> GetActiveDevicesInfo()
        => this.gamePads
            .Where(gamePad => gamePad.GetIsPluggedIn())
            .Select(gamePad => gamePad.GetInfo()).ToList();

    /// <inheritdoc />
    public void EnsurePluggedIn(int gamePadIndex)
    {
        if (gamePadIndex is < 0 or >= MAX_GAMEPADS)
        {
            throw new ArgumentOutOfRangeException(nameof(gamePadIndex));
        }

        var xInputService = ServiceLocator.Current.GetInstance<IXInputService>();
        var physicalDeviceIndexes = xInputService.GetActiveDevicesInfo();

        // If the physical device is active at the index, then we can't use that index
        if (physicalDeviceIndexes.FirstOrDefault(info => info.Index == gamePadIndex) is IGamePadInfo info)
        {
            throw new MappingArmingFailedException(
                $"There is a physical gamepad in use at index {gamePadIndex}. Cannot simulate at that index.");
        }

        var gamePad = this.gamePads[gamePadIndex];

        if (gamePad.GetIsPluggedIn())
        {
            return;
        }

        gamePad.PlugIn();
    }

    /// <inheritdoc />
    public void EnsureUnplugged(int gamePadIndex)
    {
        if (gamePadIndex is < 0 or >= MAX_GAMEPADS)
        {
            throw new ArgumentOutOfRangeException(nameof(gamePadIndex));
        }

        var xInputService = ServiceLocator.Current.GetInstance<IXInputService>();
        var physicalDeviceIndexes = xInputService.GetActiveDevicesInfo();

        // If the physical device is active at the index, then we can't use that index
        if (physicalDeviceIndexes.FirstOrDefault(info => info.Index == gamePadIndex) is IGamePadInfo info)
        {
            throw new MappingArmingFailedException(
                $"There is a physical gamepad in use at index {gamePadIndex}. Cannot simulate at that index.");
        }

        var gamePad = this.gamePads[gamePadIndex];

        if (!gamePad.GetIsPluggedIn())
        {
            return;
        }

        gamePad.Unplug();
    }

    /// <inheritdoc />
    public void EnsureAllUnplugged()
    {
        foreach (var gamePad in this.GetAllGamePads(true))
        {
            gamePad.Unplug();
        }
    }
}
