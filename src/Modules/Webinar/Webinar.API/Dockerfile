FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Modules/Webinar/Webinar.API/Webinar.API.csproj", "src/Modules/Webinar/Webinar.API/"]
RUN dotnet restore "src/Modules/Webinar/Webinar.API/Webinar.API.csproj"
COPY . .
WORKDIR "/src/src/Modules/Webinar/Webinar.API"
RUN dotnet build "Webinar.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Webinar.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webinar.API.dll"]
