FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ChatHubSolution/ChatHubSolution.csproj", "ChatHubSolution/"]
RUN dotnet restore "ChatHubSolution/ChatHubSolution.csproj"
COPY . .
WORKDIR "/src/ChatHubSolution"
RUN dotnet build "ChatHubSolution.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatHubSolution.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatHubSolution.dll"]