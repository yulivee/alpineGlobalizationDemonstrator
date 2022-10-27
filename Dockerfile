# https://hub.docker.com/_/microsoft-dotnet
# SDK for build
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.16 AS base

# install icu-libs for System.Globalization, and while we are at it, add curl for the healthcheck command
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV DOTNET_RUNNING_IN_CONTAINER=true
RUN apk add --no-cache icu-libs icu-data-full icu-dev
ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

WORKDIR /app
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-amd64 AS restore
WORKDIR /build
COPY alpinetest.csproj .
RUN dotnet restore "alpinetest.csproj" -nologo -r linux-x64

FROM restore AS build
# copy and publish app and libraries
COPY . .
WORKDIR "/build"
RUN dotnet build "alpinetest.csproj" -nologo -c Release -r linux-x64 --no-restore --no-self-contained

FROM build AS publish
RUN  dotnet publish "alpinetest.csproj" -nologo -c Release -p:UseAppHost=false --no-restore --no-build -r linux-x64 --no-self-contained -o /build/publish

# Runtime for execution
FROM base AS final
EXPOSE 80
EXPOSE 443
EXPOSE 5000
EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Container
COPY --chown=appuser --from=publish /build/publish /app
ENTRYPOINT ["dotnet", "alpinetest.dll"]