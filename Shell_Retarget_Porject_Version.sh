#!/bin/bash
find ./Tools/imgui/examples/example_win32_directx11 -name "example_win32_directx11.vcxproj" | awk -F: '{print $1}' | sort | uniq | xargs sed -i 's/v142/v141/g' 
if [ $? -ne 0 ]; then
    echo "failed"
else
    echo "succeed"
fi
