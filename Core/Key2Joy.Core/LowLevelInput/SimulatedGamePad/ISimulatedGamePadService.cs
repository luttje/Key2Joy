namespace Key2Joy.LowLevelInput.SimulatedGamePad;

/// <summary>
/// Represents a service for managing simulated gamepad devices.
/// </summary>
public interface ISimulatedGamePadService
{
    /// <summary>
    /// Gets a simulated gamepad by its index.
    /// </summary>
    /// <param name="gamePadIndex">The index of the gamepad to retrieve.</param>
    /// <returns>An instance of the simulated gamepad.</returns>
    ISimulatedGamePad GetGamePad(int gamePadIndex);

    /// <summary>
    /// Retrieves an array of all available simulated gamepad devices.
    /// </summary>
    /// <returns>An array of simulated gamepad instances.</returns>
    ISimulatedGamePad[] GetAllGamePads();

    /// <summary>
    /// Initializes the simulated gamepad service.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Shuts down the simulated gamepad service.
    /// </summary>
    void ShutDown();

    /// <summary>
    /// Ensures that a specific gamepad is plugged in.
    /// </summary>
    /// <param name="gamePadIndex">The index of the gamepad to ensure is plugged in.</param>
    void EnsurePluggedIn(int gamePadIndex);

    /// <summary>
    /// Ensures that a specific gamepad is unplugged.
    /// </summary>
    /// <param name="gamePadIndex">The index of the gamepad to ensure is unplugged.</param>
    void EnsureUnplugged(int gamePadIndex);

    /// <summary>
    /// Ensures that all simulated gamepads are unplugged.
    /// </summary>
    void EnsureAllUnplugged();
}
