#FROM microsoft/dotnet:sdk
FROM compulim/msbuild
COPY . c:\\APP
WORKDIR c:\\APP

#ADD https://download.microsoft.com/download/E/E/D/EEDF18A8-4AED-4CE0-BEBE-70A83094FC5A/BuildTools_Full.exe BuildTools_Full.exe
#RUN &  BuildTools_Full.exe /Silent /Full
SHELL ["powershell"]

