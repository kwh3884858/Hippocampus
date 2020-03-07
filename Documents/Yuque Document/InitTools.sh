#!/bin/bash

echo "Current npm Version:" &&
    npm -v

#npm install -g pomelo --proxy http://代理服务器ip:代理服务器端口
npm config set registry https://registry.npm.taobao.org

echo "Installing yuque2book"
npm install yuque2book -g

# 执行这段命令后会自动打开浏览器
echo "Installing anywhere"
npm install anywhere -g

npm install -g waque && echo "Install waque Success!"

echo "Get some documents for test usability"
yuque2book -t 73UdhDEI3Yq9JzlOyI3gy1lBm4Gl0dYqGSde0rd0 https://www.yuque.com/nxym1s/ogy3a4

cd nxym1s_ogy3a4 && anywhere
# #参数-n的作用是不换行，echo默认是换行
# echo "Press Enter to exit"
# #从键盘输入
# read anykey
#退出shell程序。
exit 0
