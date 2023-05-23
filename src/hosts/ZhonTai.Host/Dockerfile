FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["/src/hosts/ZhonTai.Host/ZhonTai.Host.csproj", "/src/hosts/ZhonTai.Host/ZhonTai.Host.csproj"]
RUN dotnet restore "/src/hosts/ZhonTai.Host/ZhonTai.Host.csproj"
COPY . .
RUN pwd
WORKDIR "/src/src/hosts/ZhonTai.Host"
ENV NUGET_XMLDOC_MODE none
RUN pwd
RUN dotnet restore "ZhonTai.Host.csproj" -nowarn:msb3202,nu1503
RUN dotnet build "ZhonTai.Host.csproj" --no-restore -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "ZhonTai.Host.csproj" --no-restore -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ZhonTai.Host.dll"]