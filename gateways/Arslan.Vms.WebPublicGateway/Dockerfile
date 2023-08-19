#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["gateways/Arslan.Vms.WebPublicGateway/Arslan.Vms.WebPublicGateway.csproj", "gateways/Arslan.Vms.WebPublicGateway/"]
RUN dotnet restore "gateways/Arslan.Vms.WebPublicGateway/Arslan.Vms.WebPublicGateway.csproj"
COPY . .
WORKDIR "/src/gateways/Arslan.Vms.WebPublicGateway"
RUN dotnet build "Arslan.Vms.WebPublicGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Arslan.Vms.WebPublicGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arslan.Vms.WebPublicGateway.dll"]