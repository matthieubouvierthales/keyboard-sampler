C:\Windows\Microsoft.Net\Framework\v4.0.30319\MSBuild.exe Sampler\Sampler.csproj
mkdir /p output
del /f  /s /q output\*.*
xcopy /s /e Sampler\bin\Debug\*.exe .\output\
xcopy /s /e Sampler\bin\Debug\*.dll .\output\
xcopy /s /e Sampler\bin\Debug\Sampler.exe.config .\output\
xcopy /s /e Sampler\bin\Debug\www\*.* .\output\www\