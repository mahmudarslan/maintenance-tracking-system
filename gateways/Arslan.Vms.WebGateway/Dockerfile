#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["gateways/Arslan.Vms.WebGateway/Arslan.Vms.WebGateway.csproj", "gateways/Arslan.Vms.WebGateway/"]
RUN dotnet restore "gateways/Arslan.Vms.WebGateway/Arslan.Vms.WebGateway.csproj"
COPY . .
WORKDIR "/src/gateways/Arslan.Vms.WebGateway"
RUN dotnet build "Arslan.Vms.WebGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Arslan.Vms.WebGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arslan.Vms.WebGateway.dll"]