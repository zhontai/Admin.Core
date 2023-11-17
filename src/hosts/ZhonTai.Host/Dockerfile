FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY ["/src/hosts/ZhonTai.Host/ZhonTai.Host.csproj", "/src/hosts/ZhonTai.Host/ZhonTai.Host.csproj"]
RUN dotnet restore "/src/hosts/ZhonTai.Host/ZhonTai.Host.csproj" -nowarn:NETSDK1138
COPY . .
WORKDIR "/src/src/hosts/ZhonTai.Host"
ENV NUGET_XMLDOC_MODE none
RUN dotnet publish "ZhonTai.Host.csproj" -nowarn:NETSDK1138 -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 8000
ENTRYPOINT ["dotnet", "ZhonTai.Host.dll"]