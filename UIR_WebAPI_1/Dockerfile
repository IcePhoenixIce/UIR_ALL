#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UIR_WebAPI_1/UIR_Service_A.csproj", "UIR_WebAPI_1/"]
RUN dotnet restore "UIR_WebAPI_1/UIR_Service_A.csproj"
COPY . .
WORKDIR "/src/UIR_WebAPI_1"
RUN dotnet build "UIR_Service_A.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UIR_Service_A.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UIR_Service_A.dll"]