@echo off
echo.
echo 安装包，生成node_modules文件
echo.

%~d0
cd %~dp0

cd ..
pnpm install --registry=https://registry.npmmirror.com

pause