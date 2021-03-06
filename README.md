# Profile
This is a Sync! plugin for displaying pp real-time you are playing osu!std,you can use them for streamming or other ways.

# Notice
Please read Readme about [Osu!RTDP](https://github.com/KedamaOvO/OsuRTDataProvider-Release) and [OsuForum](https://osu.ppy.sh/forum/t/685031) before you use.

# Settings
Settings is in config.ini<br>

| Setting Name  | Default Value | Description |
|:------------- |:-------------|:-----|
| OutputMethods | wpf | The output mode of plugin ,you can choose "wpf","mmf" and "text" (segmenting with ',' e.g: wpf,text) |
| UseText  | False | Whether to output to txt file(**recommended OutputMethods**) |
| TextOutputPath  | rtpp{0}.txt |  Output file path |
| DisplayHitObject | True  | Whether to display hitobjects(like 300_count/50_count and others) |
| PPFontSize | 48 | PP value text font size(pt) |
| PPFontColor | FFFFFFFF | PP value text color (ARGB Hex  code and no prefix '#') |
| HitObjectFontSize | 24 | hitobjects text font size(pt) |
| HitObjectFontColor | FFFFFFFF | hitobjects text color (ARGB Hex  code and no prefix '#') |
| BackgroundColor | FF00FF00 | Backgound color (default is green and good for colorkey in OBS) |
| WindowHeight | 172 | Window Height(px) |
| WindowWidth | 280 | Window Width(px) |
| SmoothTime | 200 | Time(ms) about smooth effect for updating pp |
| FPS | 60 | FPS |
| Topmost | False | Whether to PP Window is topmost(You can right click pp window) |
| WindowTextShadow | True|Whether to apply text shadow effect |
| DebugMode | False | Enable debug ouput |
| RoundDigits | 2 | accurate up to {**RoundDigits**} decimal places. |
| PPFormat | ${rtpp}pp | you can choose **rtpp rtpp_aim rtpp_speed rtpp_acc fcpp fcpp_aim fcpp_speed fcpp_acc maxpp maxpp_aim maxpp_speed maxpp_acc** [more](https://github.com/KedamaOvO/RealTimePPDisplayer/wiki/How-to-customize-my-output-content%3F)|
| HitCountFormat | ${n100}x100 ${n50}x50 ${nmiss}xMiss | you can choose **combo maxcombo fullcombo n300 n100 n50 nmiss** [more](https://github.com/KedamaOvO/RealTimePPDisplayer/wiki/How-to-customize-my-output-content%3F)|
| FontName | Segoe UI | Font name |

# How to use MMF?
1. Install [obs-text-rtpp-mmf](https://github.com/KedamaOvO/RealTimePPDisplayer/releases/download/v1.1.1/obs-text-rtpp-mmf.7z) to OBS (20.1.3).
2. Add **TextGDIPlusMMF** to scene.
3. Right click **TextGDIPlusMMF**,select **Properties**.
4. Find **Memory mapping file name**,input **rtpp**.(if tourney is enable,input rtpp{id}, e.g. rtpp0)
5. Add **mmf** to **OutputMethods** in **config.ini**, save config.ini.

**Memory mapping file name** = **内存映射文件名** = **記憶體對應檔案** = **MMF.Name**

# Request 
1. [Osu!Sync](https://github.com/Deliay/osuSync)
2. [OsuRTDataProvider](https://github.com/KedamaOvO/OsuRTDataProvider-Release)

# Preview
Tourney Mode: [Youtube](https://www.youtube.com/watch?v=begp3yimqaI)
