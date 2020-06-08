#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CodeLifter.Covid19.Admin/CodeLifter.Covid19.Admin.csproj", "CodeLifter.Covid19.Admin/"]
COPY ["CodeLifter.Covid19.Data/CodeLifter.Covid19.Data.csproj", "CodeLifter.Covid19.Data/"]
COPY ["Codelifter.IO.Github/Codelifter.IO.Github.csproj", "Codelifter.IO.Github/"]
RUN dotnet restore "CodeLifter.Covid19.Admin/CodeLifter.Covid19.Admin.csproj"
COPY . .
WORKDIR "/src/CodeLifter.Covid19.Admin"
RUN dotnet build "CodeLifter.Covid19.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeLifter.Covid19.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeLifter.Covid19.Admin.dll"]