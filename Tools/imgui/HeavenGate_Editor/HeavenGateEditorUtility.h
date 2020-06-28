//
//  HeavenGateEditorUtility.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 28/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorUtility_h
#define HeavenGateEditorUtility_h

#include <string>

typedef struct ImVec4;

namespace HeavenGateEditor {

class HeavenGateEditorUtility{

public:
    static void GetAssetPath(char* const outAssetPath);
    static void GetStoryPath(char* const outExePath);
    static void GetStoryAutoSavePath(char* const outAutoSavePath);
    static ImVec4 ConvertRGBAToFloat4(ImVec4 const originalRGBAValue);
    static unsigned int ConvertRGBAToUnsignedInt(ImVec4 const originalRGBAValue);
    //static void GetStoryExportPath(char* const outExportPath);
    #ifndef _WIN32
    static bool GetModuleFileNameOSX(char *  pOutCurrentPath);
    #endif

private:
    static std::string GetRootPath();
    static unsigned int FromRGBA(int r, int g, int b, int a);
    static void ToRGBA(unsigned int col, int &r, int &g, int &b, int &a);
};


}
#endif /* HeavenGateEditorUtility_h */
