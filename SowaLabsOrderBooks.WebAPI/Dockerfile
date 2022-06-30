#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["SowaLabsOrderBooks.WebAPI/SowaLabsOrderBooks.WebAPI.csproj", "SowaLabsOrderBooks.WebAPI/"]
COPY ["SowaLabsOrderBooks.HedgerClient/SowaLabsOrderBooks.HedgerClient.csproj", "SowaLabsOrderBooks.HedgerClient/"]
COPY ["SowaLabsOrderBooks.Models/SowaLabsOrderBooks.Models.csproj", "SowaLabsOrderBooks.Models/"]
RUN dotnet restore "SowaLabsOrderBooks.WebAPI/SowaLabsOrderBooks.WebAPI.csproj"
COPY . .
WORKDIR "/src/SowaLabsOrderBooks.WebAPI"
RUN dotnet build "SowaLabsOrderBooks.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SowaLabsOrderBooks.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SowaLabsOrderBooks.WebAPI.dll"]