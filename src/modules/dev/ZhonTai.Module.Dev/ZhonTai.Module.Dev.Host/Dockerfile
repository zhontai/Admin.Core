FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY ["/ZhonTai.Module.Dev.Host/ZhonTai.Module.Dev.Host.csproj", "/ZhonTai.Module.Dev.Host/ZhonTai.Module.Dev.Host.csproj"]
RUN dotnet restore "/ZhonTai.Module.Dev.Host/ZhonTai.Module.Dev.Host.csproj" -nowarn:NETSDK1138
COPY . .
WORKDIR "/src/ZhonTai.Module.Dev.Host"
ENV NUGET_XMLDOC_MODE none
RUN dotnet publish "ZhonTai.Module.Dev.Host.csproj" -nowarn:NETSDK1138 -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 8000
ENTRYPOINT ["dotnet", "ZhonTai.Module.Dev.Host.dll"]