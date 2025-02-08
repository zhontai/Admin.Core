FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY ["/MyApp.Host/MyApp.Host.csproj", "/MyApp.Host/MyApp.Host.csproj"]
RUN dotnet restore "/MyApp.Host/MyApp.Host.csproj" -nowarn:NETSDK1138
COPY . .
WORKDIR "/src/MyApp.Host"
ENV NUGET_XMLDOC_MODE none
RUN dotnet publish "MyApp.Host.csproj" -nowarn:NETSDK1138 -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 8000
ENTRYPOINT ["dotnet", "MyApp.Host.dll"]