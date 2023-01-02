FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Modules/Rsvp/Rsvp.API/Rsvp.API.csproj", "src/Modules/Rsvp/Rsvp.API/"]
RUN dotnet restore "src/Modules/Rsvp/Rsvp.API/Rsvp.API.csproj"
COPY . .
WORKDIR "/src/src/Modules/Rsvp/Rsvp.API"
RUN dotnet build "Rsvp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rsvp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rsvp.API.dll"]
