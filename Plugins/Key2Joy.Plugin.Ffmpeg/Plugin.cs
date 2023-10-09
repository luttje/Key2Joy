using System.IO;
using System;
using FFMpegCore;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Plugin.Ffmpeg;

public class Plugin : PluginBase
{
    public override string Name => "FFmpeg Actions";
    public override string Description => "FFmpeg Actions to generate and manipulate video and audio files.";
    public override string Author => "Luttje";
    public override string Website => "https://github.com/luttje/Key2Joy";

    public override void Initialize()
    {
        base.Initialize();

        // https://ffbinaries.com/downloads
        GlobalFFOptions.Configure(options =>
        {
            options.BinaryFolder = Path.Combine(this.PluginDirectory, "ffmpeg");
        });
    }
}
