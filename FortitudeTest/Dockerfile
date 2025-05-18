# Use the official .NET 8 ASP.NET runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Configure ASP.NET Core to listen on all interfaces
ENV ASPNETCORE_URLS=http://0.0.0.0:80

# Replace FortitudeTest.dll if your output DLL has a different name
ENTRYPOINT ["dotnet", "FortitudeTest.dll"]
