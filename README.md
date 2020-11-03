# asp.net core 5.0 demo

.net 5就要发布了，把asp.net core 3.1的一些模块改成了5.0来适配，基本都是体力活，没有太大改动。

### ORM

dapper一个轻量级的ORM，重点实现SQL语句实体映射，用sql不失灵活高效，映射用实体不失简便快捷，同时适配大部分数据库。https://github.com/axzxs2001/aspnetcore5.0/tree/master/aspnetcore5.0/Dapper

### 监控
prometheus通用监控组件，kubernetes之友https://github.com/axzxs2001/aspnetcore5.0/tree/master/aspnetcore5.0/Prometheus

### 日志

nlog简单方便的log框架，配置功能强大，无缝对接asp.net core，https://github.com/axzxs2001/aspnetcore5.0/tree/master/aspnetcore5.0/LogDemo/LogDemo01_NLog

### API文档查看
swagger被asp.net core官方web api模板内置，可见其易用，广泛。https://github.com/axzxs2001/aspnetcore5.0/tree/master/aspnetcore5.0/API/APIDemo02

### 认证授权
官方模块，利用JWT验证，进行了简单的封装，​https://github.com/axzxs2001/aspnetcore5.0/tree/master/aspnetcore5.0/AuthenticationAndAuthorization

### 健康检查
官方模块，实现了调用，是应用健康的必备
https://github.com/axzxs2001/aspnetcore5.0/tree/master/aspnetcore5.0/HealthChecks

### 本地化
做多语言的必看，官方模块，注意SharedResource.cs的位置
https://github.com/axzxs2001/aspnetcore5.0/tree/master/aspnetcore5.0/GlobalizationAndLocalization/GlobalizationLocalizationDemo01


.net5发布后，会切换成正式的包，同时也会增加我工作中用到的一些模块，我的GitHub：https://github.com/axzxs2001/aspnetcore5.0
