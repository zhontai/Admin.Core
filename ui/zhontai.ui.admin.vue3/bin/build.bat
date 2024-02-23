@echo off
echo.
echo 发布网站，生成dist文件
echo.

%~d0
cd %~dp0

cd ..
npm run build

pause