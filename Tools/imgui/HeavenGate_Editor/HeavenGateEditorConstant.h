//
//  HeavenGateEditorConstant.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 24/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorConstant_h
#define HeavenGateEditorConstant_h

namespace HeavenGateEditor {

    const int     MAX_FOLDER_PATH = 265;
    const int     MAX_FILE_NAME = 64;
    const int     MAX_FOLDER_LIST = 32;
    const int     MAX_FULL_CONTENT = 512;

    //For story content limit
    const int     MAX_NAME = 64;
    const int     MAX_CONTENT = 265;
    const int     MAX_ID = 8;

    const int     MAX_ENUM_LENGTH = 16;


    //Max number of display folders
    const int  MAX_NUM_OF_DISPLAY_FORLDERS = 50;

    //Tool folder name
    const char* const TOOL_FOLDER_NAME = "Tools";
    const char* const ASSET_FOLDER_NAME = "Assets";

    //Relative path form project root to story folder
    const char* const PATH_FROM_PROJECT_ROOT_TO_STORY_FOLDER =
#ifdef _WIN32
        "Assets\\Storys";
#else
        "Assets/Storys";
#endif

    const char* const PATH_FROM_PROJECT_ROOT_TO_FONT_FOLDER =
#ifdef _WIN32
        "Assets\\Fonts\\SourceHanSansCN-Regular.ttf";
#else
        "Assets/Fonts/SourceHanSansCN-Regular.ttf";
#endif

}
#endif /* HeavenGateEditorConstant_h */
