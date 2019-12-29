//
//  HeavenGateEditorUtility.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 28/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorUtility_h
#define HeavenGateEditorUtility_h
#include <stdio.h>

namespace HeavenGateEditor {


class HeavenGateEditorUtility{

public:
    static void GetAssetPath(char* const outAssetPath);
    static void GetStoryPath(char* const outExePath);
    //static void GetStoryExportPath(char* const outExportPath);
    #ifndef _WIN32
    static bool GetModuleFileNameOSX(char *  pOutCurrentPath);
    #endif
};


}
#endif /* HeavenGateEditorUtility_h */
