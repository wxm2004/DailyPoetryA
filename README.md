#  DailyPoetryA - Daily Poetry Avalonia

东北大学软件学院“全栈开发技术”课程教学参考项目“每日诗词”的Avalonia版本，2024年秋季课程预定！

*超级感谢无比有爱的Avalonia中文社区！*

早先版本：

* .NET MAUI Blazor Hybrid版本：https://gitee.com/zhangyin-gitee/daily-poetry-h
* .NET MAUI版本：https://gitee.com/zhangyin-gitee/daily-poetry-m
* Xamarin版本：https://gitee.com/zhangyin-gitee/DailyPoetryX

## 已知局限

本项目是一个教学参考项目，不是一个适用于生产环境的应用程序。它的目的是帮助学生理解Avalonia的基本原理和开发方法，并借此学习一系列关键且通用的技术和思想如依赖注入、异步操作、MVVM以及反射等等。受限于以下已知的以及更多未列出的局限，该项目不能作为Avalonia的最佳实践：

* Avalonia官方似乎推荐使用ReactiveUI来实现MVVM，但ReactiveUI的学习曲线较陡。受限于有限的课时，本项目没有使用ReactiveUI，而是使用了CommunityToolkit.Mvvm。
* 许多社区成员都推荐使用Prism来实现依赖注入及导航等功能。不过为了方便对依赖注入和导航的原理开展学习，本项目选择了一种更轻量化的实现。
* 本项目所有的ViewModel以及Service都是单例的。在实际应用中，这种设计一定会带问题。
* 为了控制项目的复杂度，本项目没有考虑线程冲突以及操作的原子性等问题。

## 参考链接

* https://github.com/irihitech/Semi.Avalonia
* https://github.com/irihitech/Ursa.Avalonia
* https://github.com/Coolkeke/LayUI-Avalonia