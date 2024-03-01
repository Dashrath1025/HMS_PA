#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
ENV ASPNETCORE_ENVIRONMENT = Development
COPY ["Hospital.Services.Clinic/Hospital.Services.Clinic.csproj", "Hospital.Services.Clinic/"]
RUN dotnet restore "Hospital.Services.Clinic/Hospital.Services.Clinic.csproj"
COPY . .
WORKDIR "/src/Hospital.Services.Clinic"
RUN dotnet build "Hospital.Services.Clinic.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hospital.Services.Clinic.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hospital.Services.Clinic.dll","--environment=Development"]