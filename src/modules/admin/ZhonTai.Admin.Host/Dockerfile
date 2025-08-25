FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
WORKDIR /src
COPY ["/src/modules/admin/ZhonTai.Admin.Host/ZhonTai.Admin.Host.csproj", "/src/modules/admin/ZhonTai.Admin.Host/ZhonTai.Admin.Host.csproj"]
RUN dotnet restore "/src/modules/admin/ZhonTai.Admin.Host/ZhonTai.Admin.Host.csproj" -nowarn:NETSDK1138
COPY . .
WORKDIR "/src/src/modules/admin/ZhonTai.Admin.Host"
ENV NUGET_XMLDOC_MODE none
RUN dotnet publish "ZhonTai.Admin.Host.csproj" -nowarn:NETSDK1138 -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 18010
ENTRYPOINT ["dotnet", "ZhonTai.Admin.Host.dll"]