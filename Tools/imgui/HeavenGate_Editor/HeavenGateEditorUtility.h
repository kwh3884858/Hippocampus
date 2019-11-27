//
//  HeavenGateEditorUtility.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 28/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorUtility_h
#define HeavenGateEditorUtility_h

namespace HeavenGateEditor {

#include <stdio.h>



class HeavenGateEditorUtility{

public:

    static void GetStoryPath(char* const outExePath);

    #ifndef _WIN32
    static bool GetModuleFileNameOSX(char *  pOutCurrentPath);
    #endif
};


}
#endif /* HeavenGateEditorUtility_h */
