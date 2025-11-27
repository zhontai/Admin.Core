# 使用官方 ASP.NET Core 10.0 运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS publish
# 设置工作目录为 /app
WORKDIR /app
# 暴露容器端口 18010
EXPOSE 18010
# 将构建上下文（通常是项目目录）中的所有文件复制到容器的 /app 目录
COPY . .
# 设置容器时区为亚洲/上海（UTC+8）
ENV TZ=Asia/Shanghai
# 设置容器启动时执行的命令：运行 *.Host.dll 文件
ENTRYPOINT ["dotnet", "ZhonTai.Admin.Host.dll"]