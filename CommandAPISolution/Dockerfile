FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CommandAPISolution/CommandAPISolution.csproj", "CommandAPISolution/"]
RUN dotnet restore "CommandAPISolution/CommandAPISolution.csproj"
COPY . .
WORKDIR "/src/CommandAPISolution"
RUN dotnet build "CommandAPISolution.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommandAPISolution.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommandAPISolution.dll"]
