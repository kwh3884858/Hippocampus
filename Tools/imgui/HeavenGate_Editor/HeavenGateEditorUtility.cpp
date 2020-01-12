//
//  HeavenGateEditorUtility.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 28/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#include "HeavenGateEditorUtility.h"
#include "HeavenGateEditorConstant.h"

#include "CharacterUtility.h"

#ifdef _WIN32
#include <windows.h>

#else
//For Dl_info
#include <dlfcn.h>

#endif // _WIN32



using std::string;

namespace HeavenGateEditor {
    void HeavenGateEditorUtility::GetAssetPath(char * const outAssetPath)
    {

    }
    void HeavenGateEditorUtility::GetStoryPath(char* const pOutExePath) {

        char cBuffer[MAX_FOLDER_PATH];

#ifdef _WIN32

        wchar_t buffer[MAX_FOLDER_PATH];
        GetModuleFileName(NULL, buffer, MAX_FOLDER_PATH);
        CharacterUtility::convertWcsToMbs(cBuffer, buffer, MAX_FOLDER_PATH);
#else
        bool result = GetModuleFileNameOSX(cBuffer);

        if (!result) {
            return;
        }
#endif

        //TODO: Need to write a string function to find tool folder name
        string path(cBuffer);
        string::size_type pos = path.find(TOOL_FOLDER_NAME);

        path = path.substr(0, pos);
        path = path.append(PATH_FROM_PROJECT_ROOT_TO_STORY_FOLDER);
        printf("  %s  \n", path.c_str());
        strcpy(pOutExePath, path.c_str());

        return;
    }

    ImVec4 HeavenGateEditorUtility::ConvertRGBAToFloat4(ImVec4 const originalRGBAValue)
    {
        float x = originalRGBAValue.x / COLOR_RGBA_BASE_VALUE;
        float y = originalRGBAValue.y / COLOR_RGBA_BASE_VALUE;
        float z = originalRGBAValue.z / COLOR_RGBA_BASE_VALUE;
        float w = originalRGBAValue.w / COLOR_RGBA_BASE_VALUE;
        ImVec4 outV4 = ImVec4(x, y, z, w);
        return outV4;
    }



    //    void HeavenGateEditorUtility::GetStoryExportPath(char* const outExportPath)
    //    {
    //        char cBuffer[MAX_FOLDER_PATH];
    //
    //#ifdef _WIN32
    //
    //        wchar_t buffer[MAX_FOLDER_PATH];
    //        GetModuleFileName(NULL, buffer, MAX_FOLDER_PATH);
    //        CharacterUtility::convertWcsToMbs(cBuffer, buffer, MAX_FOLDER_PATH);
    //#else
    //        bool result = GetModuleFileNameOSX(cBuffer);
    //
    //        if (!result) {
    //            return;
    //        }
    //#endif
    //
    //        //TODO: Need to write a string function to find tool folder name
    //        string path(cBuffer);
    //        string::size_type pos = path.find(TOOL_FOLDER_NAME);
    //
    //        path = path.substr(0, pos);
    //        path = path.append(PATH_FROM_PROJECT_ROOT_TO_STORY_EXPORT_FOLDER);
    //        printf("  %s  \n", path.c_str());
    //        strcpy(outExportPath, path.c_str());
    //
    //        return;
    //    }

#ifndef _WIN32
    bool HeavenGateEditorUtility::GetModuleFileNameOSX(char* pOutCurrentPath) {
        Dl_info module_info;
        if (dladdr(reinterpret_cast<void*>(GetModuleFileNameOSX), &module_info) == 0) {
            // Failed to find the symbol we asked for.
            return false;
        }

        CharacterUtility::copyCharPointer(pOutCurrentPath, module_info.dli_fname);
        return  true;
    }
#endif
}
