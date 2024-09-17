#  DailyPoetryA - Daily Poetry Avalonia

*超级感谢无比有爱的Avalonia中文社区！*

## 设计理念与功能目标

DPA 的设计理念是为用户提供一种轻松愉快的诗词欣赏体验，同时弘扬经典诗词文化。以下是该应用的一些主要功能和目标：

1. 每日诗词推送：应用每天自动从内置数据库中选择一首诗词进行展示，用户也可以手动浏览过去的诗词。未来版本可能会增加在线诗词更新和推荐功能。
2. 图片与诗词结合：诗词展示不仅仅是文字，应用还为每首诗词提供了与之呼应的图片，增强了用户的视觉体验。
3. 离线访问：所有诗词和图片都存储在本地数据库中，用户无需网络连接即可使用应用程序，特别适合喜欢安静阅读的用户。
4. MVVM 设计模式：应用使用了 Model-View-ViewModel 设计模式，将界面与逻辑完全解耦，便于维护和扩展。通过依赖注入和服务定位器的使用，未来可以轻松集成新功能或扩展现有功能。
5. 跨平台支持：通过 Avalonia UI 框架，应用可以在多个操作系统（Windows、macOS、Linux）上无缝运行。无论用户使用何种设备，都可以享受到一致的用户体验。

## 已知局限

* Avalonia官方似乎推荐使用ReactiveUI来实现MVVM，但ReactiveUI的学习曲线较陡。受限于有限的课时，本项目没有使用ReactiveUI，而是使用了CommunityToolkit.Mvvm。
* 许多社区成员都推荐使用Prism来实现依赖注入及导航等功能。不过为了方便对依赖注入和导航的原理开展学习，本项目选择了一种更轻量化的实现。
* 本项目所有的ViewModel以及Service都是单例的。在实际应用中，这种设计一定会带问题。
* 为了控制项目的复杂度，本项目没有考虑线程冲突以及操作的原子性等问题。

## 参考链接

* https://github.com/irihitech/Semi.Avalonia
* https://github.com/irihitech/Ursa.Avalonia
* https://github.com/Coolkeke/LayUI-Avalonia
