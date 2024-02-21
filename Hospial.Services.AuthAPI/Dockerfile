#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV ASPNETCORE_ENVIRONMENT = Development
WORKDIR /src
COPY ["Hospial.Services.AuthAPI/Hospital.Services.AuthAPI.csproj", "Hospial.Services.AuthAPI/"]
RUN dotnet restore "Hospial.Services.AuthAPI/Hospital.Services.AuthAPI.csproj"
COPY . .
WORKDIR "/src/Hospial.Services.AuthAPI"
RUN dotnet build "Hospital.Services.AuthAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hospital.Services.AuthAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hospital.Services.AuthAPI.dll","--environment=Development"]