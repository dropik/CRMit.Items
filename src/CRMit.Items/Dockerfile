FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
COPY /src /src/
WORKDIR "/src/CRMit.Items"
RUN dotnet restore "CRMit.Items.csproj"
RUN dotnet build "CRMit.Items.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRMit.Items.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRMit.Items.dll"]