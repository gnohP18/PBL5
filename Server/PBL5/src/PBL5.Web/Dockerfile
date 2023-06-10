#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/PBL5.Web/PBL5.Web.csproj", "src/PBL5.Web/"]
COPY ["src/PBL5.Application/PBL5.Application.csproj", "src/PBL5.Application/"]
COPY ["src/PBL5.Domain/PBL5.Domain.csproj", "src/PBL5.Domain/"]
COPY ["src/PBL5.Domain.Shared/PBL5.Domain.Shared.csproj", "src/PBL5.Domain.Shared/"]
COPY ["src/PBL5.Application.Contracts/PBL5.Application.Contracts.csproj", "src/PBL5.Application.Contracts/"]
COPY ["src/PBL5.HttpApi/PBL5.HttpApi.csproj", "src/PBL5.HttpApi/"]
COPY ["src/PBL5.EntityFrameworkCore/PBL5.EntityFrameworkCore.csproj", "src/PBL5.EntityFrameworkCore/"]
RUN dotnet restore "src/PBL5.Web/PBL5.Web.csproj"
COPY . .
WORKDIR "/src/src/PBL5.Web"
RUN dotnet build "PBL5.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PBL5.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PBL5.Web.dll"]