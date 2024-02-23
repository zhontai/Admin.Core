@echo off
echo.
echo 运行网站
echo.

%~d0
cd %~dp0

cd ..
npm run dev

pause