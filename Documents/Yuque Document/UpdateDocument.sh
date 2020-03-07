#!/bin/bash

echo "Current npm Version:" &&
    npm -v

rm -rf ./nxym1s_bf66yw/
rm -rf ./nxym1s_neu8a9/
rm -rf ./nxym1s_ogy3a4/

rm -rf ./原爆点-历史与设定/
rm -rf ./原爆点-工程文档/
rm -rf ./监狱岛-剧情文本/

yuque2book -t 73UdhDEI3Yq9JzlOyI3gy1lBm4Gl0dYqGSde0rd0 https://www.yuque.com/nxym1s/ogy3a4 &&
    mv nxym1s_ogy3a4 原爆点-历史与设定

yuque2book -t 73UdhDEI3Yq9JzlOyI3gy1lBm4Gl0dYqGSde0rd0 https://www.yuque.com/nxym1s/bf66yw &&
    mv nxym1s_bf66yw 原爆点-工程文档

yuque2book -t 73UdhDEI3Yq9JzlOyI3gy1lBm4Gl0dYqGSde0rd0 https://www.yuque.com/nxym1s/neu8a9 &&
    mv nxym1s_neu8a9 监狱岛-剧情文本

cd 监狱岛-剧情文本 && anywhere
# #参数-n的作用是不换行，echo默认是换行

#退出shell程序。
exit 0
