#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SUPER_DUPER_MEGA_BOT/SUPER_DUPER_MEGA_BOT.csproj", "SUPER_DUPER_MEGA_BOT/"]
RUN dotnet restore "SUPER_DUPER_MEGA_BOT/SUPER_DUPER_MEGA_BOT.csproj"
COPY . .
WORKDIR "/src/SUPER_DUPER_MEGA_BOT"
RUN dotnet build "SUPER_DUPER_MEGA_BOT.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SUPER_DUPER_MEGA_BOT.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SUPER_DUPER_MEGA_BOT.dll"]