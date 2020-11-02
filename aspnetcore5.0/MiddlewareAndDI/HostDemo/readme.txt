
一、配置
1、在项目文件中
  <PropertyGroup>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <OutputType>Exe</OutputType>
  </PropertyGroup>
2、Project.cs文件中
 var pathToContentRoot = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
 return Host.CreateDefaultBuilder(args)
       //run window service
       .UseWindowsService()
       .UseContentRoot(pathToContentRoot)

       .ConfigureWebHostDefaults(webBuilder =>
       {
           webBuilder.UseStartup<Startup>();
       });

二、服务管理
1、创建服务
sc create HostDemo binPath= "d:/DataTransfer/HostDemo.exe"

2、删除服务
sc delete HostDemo

3、开始服务
sc start HostDemo

4、停止服务
sc stop HostDemo

5、查看服务状态
sc query HostDemo



