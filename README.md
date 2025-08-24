应b友要求，我把 Revolution Idle 的自制模组（修改器）公布出来

## 写在前面

使用该模组可能出现封号(禁止参与排行榜)、存档崩坏(比如某个量变成NaN)等情况，开发者不承担相应责任

（封号是拿ce乱搞的时候触发的，用模组暂时没遇到，但我不能保证；出现NaN是在进行三重膨胀挑战的时候，顶上的数字会变成NaN，虽然没有发生不良后果，但我不能保证；还有就是以前因为改得太猛了，数字太大会把圈速炸掉变成NaN，然后所有的生成器、自动化都会停止）

我不保证所有人都能使用这个模组，我马上就开学了，开学就高三了，所以我很可能没时间解决产生的问题

有什么问题、建议、想法可以发发 Issues，我如果有空会看看的

## 使用方法
1. 访问 https://builds.bepinex.dev/projects/bepinex_be 

    向下翻，找到 Artifacts **#738** ，根据你的系统下载合适的版本

    (Windows x64 可直接点[这里](https://builds.bepinex.dev/projects/bepinex_be/738/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.738%2Baf0cba7.zip))

2. 将压缩包的**所有文件**解压到游戏根目录(将 winhttp.dll 等文件直接解压到 Revolution Idle.exe 的**同级目录**)

3. 访问 [Releases](https://github.com/Misaka10049/rev-idle/releases/latest) ，下载 Trainer_for_Revolution_Idle.dll

4. 打开 游戏目录/BepInEx/plugins 文件夹，将下载的 dll 文件复制进去

5. 启动游戏，如果看到控制台，说明 BepInEx 安装成功，安装 BepInEx 后的第一次启动需要 1~2 分钟，这是 BepInEx 在分析游戏的类和方法等，耐心等待，后续启动时长会恢复正常

    如果控制台出现白字 "Plugin Trainer_for_Revolution_Idle was loaded!"，说明修改器成功加载

6. 关闭游戏，打开 游戏目录/BepInEx/config/trainer.ri.cfg ，修改配置文件，里面注明了各项功能(示例见下)

7. 重新启动游戏，享受破解快乐

## trainer.ri.cfg 示例

```
## Settings file was created by plugin Trainer v0.1.0
## Plugin GUID: trainer.ri

[Inventory]

## 将灵魂数量锁定为 soul, 0 为不启用, 最大为 2147483647
# Setting type: Int32
# Default value: 0
soul = 0

[Revolution]

## 每帧使每圈多完成 10^boostProgress 次, -1 为关闭, 最大为 300
# Setting type: Int32
# Default value: -1
boostProgress = -1

[Time]

## 将时间流量锁定为 timeFlux 秒, 0 为不启用, 最大为 2147483647
# Setting type: Int32
# Default value: 0
timeFlux = 0

[Unity]

## 将星尘数量锁定为 10^astrodust, 0 为不启用, 最大为 300
# Setting type: Int32
# Default value: 0
astrodust = 0

## 每帧获得金子，数量等同于统一时能获得的金子, false 为不启用, true 为启用
# Setting type: Boolean
# Default value: false
goldOnUnity = false

[Unity.Mine]

## 每帧获得抛光点数，数量等同于抛光时能获得的抛光点数, false 为不启用, true 为启用
# Setting type: Boolean
# Default value: false
polishPointsNext = false

## 每帧获得精炼点数，数量等同于精炼时能获得的精炼点数, false 为不启用, true 为启用
# Setting type: Boolean
# Default value: false
refinePointsNext = false

## 自动合成矿物, false 为不启用, true 为启用
# Setting type: Boolean
# Default value: false
AutoMerge = false

```
