FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["OAuth/OAuth.csproj", "OAuth/"]
RUN dotnet restore "OAuth/OAuth.csproj"
COPY . .
WORKDIR "/src/OAuth"
RUN dotnet build "OAuth.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OAuth.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OAuth.dll"]