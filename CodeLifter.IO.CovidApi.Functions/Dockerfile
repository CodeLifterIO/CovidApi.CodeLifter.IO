#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CodeLifter.IO.CovidApi.Functions/CodeLifter.IO.CovidApi.Functions.csproj", "CodeLifter.IO.CovidApi.Functions/"]
COPY ["CodeLifter.Covid19.Data/CodeLifter.Covid19.Data.csproj", "CodeLifter.Covid19.Data/"]
COPY ["Codelifter.IO.Github/Codelifter.IO.Github.csproj", "Codelifter.IO.Github/"]
RUN dotnet restore "CodeLifter.IO.CovidApi.Functions/CodeLifter.IO.CovidApi.Functions.csproj"
COPY . .
WORKDIR "/src/CodeLifter.IO.CovidApi.Functions"
RUN dotnet build "CodeLifter.IO.CovidApi.Functions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeLifter.IO.CovidApi.Functions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true