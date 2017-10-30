set MSBUILD=C:\Program Files (x86)\MSBuild\14.0\Bin

call "%MSBUILD%\MSBuild.exe" CardMaster.sln /t:Clean;Build /p:Configuration=Release