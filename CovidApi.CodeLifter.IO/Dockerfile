#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY CovidApi.CodeLifter.IO/CovidApi.CodeLifter.IO.csproj CovidApi.CodeLifter.IO/
COPY CodeLifter.Covid19.Data/CodeLifter.Covid19.Data.csproj CodeLifter.Covid19.Data/
RUN dotnet restore "CovidApi.CodeLifter.IO/CovidApi.CodeLifter.IO.csproj"
COPY . .
WORKDIR "/src/CovidApi.CodeLifter.IO"
RUN dotnet build "CovidApi.CodeLifter.IO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CovidApi.CodeLifter.IO.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CovidApi.CodeLifter.IO.dll"]
