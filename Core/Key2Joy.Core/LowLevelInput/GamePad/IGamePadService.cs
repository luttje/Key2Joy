namespace Key2Joy.LowLevelInput.GamePad;

public interface IGamePadService
{
    IGamePad GetGamePad(int gamePadIndex);

    IGamePad[] GetAllGamePads();

    void Initialize();

    void ShutDown();

    void EnsurePluggedIn(int gamePadIndex);

    void EnsureUnplugged(int gamePadIndex);

    void EnsureAllUnplugged();
}
